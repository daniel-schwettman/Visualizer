using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace Visualizer.Util
{
    public partial class ImageData
    {
        private int width;
        private int height;

        private byte[] data;
        private ImageType imageType;

        public static byte[] BitCountLookupTable;

        public int UncompressedLength { get; set; }
        //public bool IsCompressed { get; private set; }

        static ImageData()
        {
            ImageData.BitCountLookupTable = new byte[256];

            ImageData.BitCountLookupTable[0] = 0;
            for (byte i = 1; i != 0; ++i)
            {
                BitArray bitArray = new BitArray(new byte[] { i });
                for (int index = 0; index < 8; ++index)
                {
                    if (bitArray.Get(index) == true)
                    {
                        ImageData.BitCountLookupTable[i]++;
                    }
                }
            }
            //CacheBitmapSourceReflectionObjects();
        }

        public ImageData(ImageData source)
        {
            this.width = source.width;
            this.height = source.height;
            this.imageType = source.imageType;
            this.UncompressedLength = source.UncompressedLength;

            int byteLength = source.data.Length;

            //try to handle case where system is out of memory
            try
            {
                this.data = new byte[byteLength];
            }
            catch (Exception ex)
            {
                GC.Collect();

                // An exception will be thrown if still OOM
                this.data = new byte[byteLength];
            }

            Buffer.BlockCopy(source.data, 0, this.data, 0, byteLength);
        }

        public ImageData(int width, int height, ImageType imageType, byte[] data)
        {
            this.width = width;
            this.height = height;
            this.imageType = imageType;
            this.data = data;
        }

        internal void SetContent(int width, int height, ImageType imageType, byte[] data)
        {
            this.width = width;
            this.height = height;
            this.imageType = imageType;
            this.data = data;
        }

        public ImageData(int width, int height, ImageType imageType)
        {
            this.width = width;
            this.height = height;
            this.imageType = imageType;

            switch (this.imageType)
            {
                case ImageType.Monochrome:
                    {
                        if (this.width % 8 != 0)
                        {
                            throw new InvalidOperationException(String.Format("Image width must be multiple of 8: {0}", this.width));
                        }
                        int length = this.width * this.height / 8;
                        this.data = new byte[length];
                    }
                    break;
                case ImageType.Grayscale:
                    {
                        int length = this.width * this.height;
                        this.data = new byte[length];
                    }
                    break;
                case ImageType.TrueColor:
                    {
                        int length = 3 * this.width * this.height;
                        this.data = new byte[length];
                    }
                    break;
                case ImageType.Pbgra:
                    {
                        int length = 4 * this.width * this.height;
                        this.data = new byte[length];
                        for (int pixel = 0; pixel < length; ++pixel)
                        {
                            this.data[pixel] = 255;
                        }
                    }
                    break;
                case ImageType.Abgr:
                    {
                        int length = 4 * this.width * this.height;
                        this.data = new byte[length];
                        for (int pixel = 0; pixel < length / 4; ++pixel)
                        {
                            int x = pixel * 4;
                            this.data[x] = 0;
                            this.data[x + 1] = 255;
                            this.data[x + 2] = 255;
                            this.data[x + 3] = 255;
                        }
                    }
                    break;
                case ImageType.Cmyk:
                    {
                        int length = 4 * this.width * this.height;
                        this.data = new byte[length];
                        for (int pixel = 0; pixel < length; ++pixel)
                        {
                            this.data[pixel] = 0;
                        }
                    }
                    break;
                default:
                    throw new InvalidOperationException(String.Format("Unsupported image type: {0}", this.imageType));
            }
        }

        public ImageData(Bitmap image)
        {
            switch (image.PixelFormat)
            {
                case PixelFormat.Format1bppIndexed:
                    this.imageType = ImageType.Monochrome;
                    break;
                case PixelFormat.Format8bppIndexed:
                    this.imageType = ImageType.Grayscale;
                    break;
                case PixelFormat.Format24bppRgb:
                    this.imageType = ImageType.TrueColor;
                    break;
                default:
                    throw new InvalidOperationException(String.Format("Unsupported image depth: {0}", image.PixelFormat.ToString()));
            }

            this.width = image.Width;
            this.height = image.Height;

            switch (this.imageType)
            {
                case ImageType.Monochrome:
                    {
                        BitmapData imageData = image.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format1bppIndexed);

                        int byteWidth = this.width / 8;
                        int length = byteWidth * this.height;
                        this.data = new byte[length];

                        unsafe
                        {
                            fixed (byte* dst = this.data)
                            {
                                byte* pd = dst;

                                for (int y = 0; y < this.height; ++y)
                                {
                                    byte* ps = (byte*)imageData.Scan0 + y * imageData.Stride;

                                    for (int x = 0; x < byteWidth; ++x)
                                    {
                                        if (*ps != 0)
                                        {
                                            *pd = *ps;
                                        }

                                        pd++;
                                        ps++;
                                    }
                                }
                            }
                        }

                        image.UnlockBits(imageData);
                    }
                    break;
                case ImageType.Grayscale:
                    {
                        int length = this.width * this.height;
                        this.data = new byte[length];

                        BitmapData imageData = image.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);

                        int sourceOffset = imageData.Stride - width;

                        unsafe
                        {
                            fixed (byte* dst = data)
                            {
                                byte* src = (byte*)imageData.Scan0;

                                byte* ps = src;
                                byte* pd = dst;

                                for (int y = 0; y < height; ++y)
                                {
                                    for (int x = 0; x < width; x++)
                                    {
                                        *pd = *ps;
                                        pd++;
                                        ps++;
                                    }

                                    ps += sourceOffset;
                                }
                            }
                        }
                        image.UnlockBits(imageData);
                    }
                    break;
                case ImageType.TrueColor:
                    {
                        int length = 3 * this.width * this.height;
                        this.data = new byte[length];

                        BitmapData imageData = image.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                        int sourceOffset = imageData.Stride - width * 3;

                        unsafe
                        {
                            fixed (byte* dst = data)
                            {
                                byte* src = (byte*)imageData.Scan0;

                                byte* ps = src;
                                byte* pd = dst;

                                for (int y = 0; y < height; ++y)
                                {
                                    for (int x = 0; x < width * 3; x++)
                                    {
                                        *pd = *ps;
                                        pd++;
                                        ps++;
                                    }

                                    ps += sourceOffset;
                                }
                            }
                        }
                        image.UnlockBits(imageData);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("imageType", this.imageType, "Unsupported image type");
            }
        }


        public ImageData Clone()
        {
            return new ImageData(this);
        }

        public ImageData TopHalf()
        {
            byte[] topData = new byte[this.data.Length / 2];
            Array.Copy(this.data, 0, topData, 0, topData.Length);
            return new ImageData(this.width, this.height / 2, this.imageType, topData);
        }
        public ImageData BottomHalf()
        {
            byte[] bottomData = new byte[this.data.Length / 2];
            Array.Copy(this.data, bottomData.Length, bottomData, 0, bottomData.Length);
            return new ImageData(this.width, this.height / 2, this.imageType, bottomData);
        }

#if NOTNEEDEDYET
        public ImageData ToArgb()
        {
            if (this.ImageType != Imaging.ImageType.TrueColor)
            {
                throw new InvalidOperationException("Only RGB images can be converted to Argb");
            }
            byte[] dataArgb = new byte[(this.width + 1) * this.height];
            int destinationPos = 0;
            for (int i = 0; i < this.data.Length; i+=3)
            {
                //data[destinationPos] = 0;
                dataArgb[destinationPos + 1] = this.data[i];
                dataArgb[destinationPos + 2] = this.data[i + 1];
                dataArgb[destinationPos + 3] = this.data[i + 2];
                destinationPos += 4;
            }
            return new ImageData(this.width, this.height, Imaging.ImageType.Argb, dataArgb);
        }
#endif

        public int CountPixels()
        {
            if (this.imageType != ImageType.Monochrome)
            {
                throw new InvalidOperationException("Only monochrome pixel counting is supported");
            }

            int totalBits = 0;
            int length = this.data.Length;
            for (int byteIndex = 0; byteIndex < length; ++byteIndex)
            {
                totalBits += ImageData.BitCountLookupTable[this.data[byteIndex]];
            }

            return totalBits;
        }

        public Bitmap CreateBitmap()
        {
            switch (this.imageType)
            {
                case ImageType.Monochrome:
                    {
                        Bitmap image = new Bitmap(this.width, this.height, PixelFormat.Format1bppIndexed);
                        BitmapData imageData = image.LockBits(new System.Drawing.Rectangle(0, 0, this.width, this.height), ImageLockMode.ReadWrite, PixelFormat.Format1bppIndexed);

                        int byteWidth = this.width / 8;

                        unsafe
                        {
                            fixed (byte* src = data)
                            {
                                byte* ps = src;

                                for (int y = 0; y < this.height; ++y)
                                {
                                    byte* pd = (byte*)imageData.Scan0 + y * imageData.Stride;

                                    for (int x = 0; x < byteWidth; ++x)
                                    {
                                        if (*ps != 0)
                                        {
                                            *pd = *ps;
                                        }

                                        pd++;
                                        ps++;
                                    }
                                }
                            }
                        }

                        image.UnlockBits(imageData);
                        return image;
                    }
                case ImageType.Grayscale:
                    {
                        Bitmap image = new Bitmap(this.width, this.height, PixelFormat.Format8bppIndexed);
                        BitmapData imageData = image.LockBits(new System.Drawing.Rectangle(0, 0, this.width, this.height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

                        int destinationOffset = imageData.Stride - width;

                        unsafe
                        {
                            fixed (byte* src = data)
                            {
                                byte* dst = (byte*)imageData.Scan0;

                                byte* ps = src;
                                byte* pd = dst;

                                for (int y = 0; y < height; ++y)
                                {
                                    for (int x = 0; x < width; x++)
                                    {
                                        *pd = *ps;
                                        pd++;
                                        ps++;
                                    }

                                    pd += destinationOffset;
                                }
                            }
                        }
                        image.UnlockBits(imageData);
                        ColorPalette pal = image.Palette;
                        for (int i = 0; i < 256; i++)
                        {
                            pal.Entries[i] = Color.FromArgb(255, i, i, i);
                        }
                        image.Palette = pal;
                        return image;
                    }
                case ImageType.TrueColor:
                    {
                        Bitmap image = new Bitmap(this.width, this.height, PixelFormat.Format24bppRgb);
                        BitmapData imageData = image.LockBits(new System.Drawing.Rectangle(0, 0, this.width, this.height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                        int destinationOffset = imageData.Stride - width * 3;

                        unsafe
                        {
                            fixed (byte* src = data)
                            {
                                byte* dst = (byte*)imageData.Scan0;

                                byte* ps = src;
                                byte* pd = dst;

                                for (int y = 0; y < height; ++y)
                                {
                                    for (int x = 0; x < width * 3; x++)
                                    {
                                        *pd = *ps;
                                        pd++;
                                        ps++;
                                    }

                                    pd += destinationOffset;
                                }
                            }
                        }
                        image.UnlockBits(imageData);
                        return image;
                    }
                default:
                    throw new ArgumentOutOfRangeException("imageType", imageType, "Unsupported image type");
            }
        }

        public ImageData ApplyFilter(IFilter filter)
        {
            return filter.ApplyFilter(this);
        }

        public void ApplyFilterInPlace(IFilter filter)
        {
            filter.ApplyFilterInPlace(this);
        }

        public void BitBlt(ImageData image, int offsetX, int offsetY)
        {
            switch (this.imageType)
            {
                case ImageType.Monochrome:
                    {
                        if (image.ImageType != ImageType.TrueColor)
                        {
                            throw new InvalidOperationException(String.Format("Unsupported BitBlt source format: {0}", image.ImageType));
                        }

                        int strideWidth = this.width / 8;

                        unsafe
                        {
                            fixed (byte* src = image.Data)
                            fixed (byte* dst = this.data)
                            {
                                byte* pd = dst;
                                byte* ps1 = src;

                                int maxY = image.Height;

                                //if the source image bottom exceeds the bounds of the destination bottom 
                                if (image.Height + offsetY > this.height)
                                {
                                    //make the maximum source height be such that the source height + y offset
                                    //goes to but does not exceed the destination's bottom
                                    maxY = this.height - offsetY;
                                }

                                int maxX = image.Width;

                                //if the source image right edge exceeds the bounds of the destination right edge 
                                if (image.Width + offsetX >= this.width)
                                {
                                    //make the maximum source right edge be such that the source right edge + x offset
                                    //goes to but does not exceed the destination's right edge
                                    maxX = this.width - offsetX;
                                }

                                //for each row in the source image
                                for (int y = 0; y < maxY; y++)
                                {
                                    //get the pointer to the source data on the current row
                                    byte* ps = (byte*)(ps1 + y * image.Width * 3);

                                    //the destination row
                                    int destinationY = y + offsetY;

                                    //for each source 'pixel'
                                    for (int x = 0; x < maxX; x++)
                                    {
                                        int pixel = 3 * ps[0] + 4 * ps[1] + 2 * ps[2];
                                        //pixel = (pixel >> 3);

                                        //if we should make the destination ON
                                        if (pixel < 1024) //use 1024 instead of 128, so that pixel does not need right shifted by 3
                                        {
                                            //the destination horizontal column
                                            int destinationX = x + offsetX;

                                            //the mask that is used to turn the single bit on
                                            byte mask = (byte)(0x80 >> (destinationX & 0x7));

                                            //apply the mask to the proper destination location
                                            pd[destinationY * strideWidth + (destinationX >> 3)] |= mask;
                                        }
                                        ps += 3;
                                    }
                                }
                            }
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("imageType", this.imageType, "Unsupported destination BitBlt");
            }
        }


        public void BitBlt(Bitmap image, int offsetX, int offsetY)
        {
            BitBlt(image, offsetX, offsetY, MaskType.None);
        }
        public void BitBlt(Bitmap image, int offsetX, int offsetY, MaskType maskType)
        {
            BitBlt(image, offsetX, offsetY, maskType, 128 << 3);
        }

        public void BitBlt(Bitmap image, int offsetX, int offsetY, MaskType maskType, int threshold)
        {
            switch (this.imageType)
            {
                case ImageType.Monochrome:
                    {
                        if (image.PixelFormat != PixelFormat.Format24bppRgb)
                        {
                            throw new InvalidOperationException(String.Format("Unsupported BitBlt source format: {0}", image.PixelFormat));
                        }

                        BitmapData sourceData = image.LockBits(new System.Drawing.Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                        int strideWidth = this.width / 8;

                        unsafe
                        {
                            fixed (byte* dst = this.data)
                            {
                                byte* pd = dst;

                                int maxY = image.Height;

                                //if the source image bottom exceeds the bounds of the destination bottom 
                                if (image.Height + offsetY > this.height)
                                {
                                    //make the maximum source height be such that the source height + y offset
                                    //goes to but does not exceed the destination's bottom
                                    maxY = this.height - offsetY;
                                }

                                int maxX = image.Width;

                                //if the source image right edge exceeds the bounds of the destination right edge 
                                if (image.Width + offsetX >= this.width)
                                {
                                    //make the maximum source right edge be such that the source right edge + x offset
                                    //goes to but does not exceed the destination's right edge
                                    maxX = this.width - offsetX;
                                }

                                switch (maskType)
                                {
                                    case MaskType.None:
                                        {
                                            //for each row in the source image
                                            for (int y = 0; y < maxY; y++)
                                            {
                                                //get the pointer to the source data on the current row
                                                byte* ps = (byte*)sourceData.Scan0 + y * sourceData.Stride;

                                                //the destination row
                                                int destinationY = y + offsetY;

                                                //for each source 'pixel'
                                                for (int x = 0; x < maxX; x++)
                                                {
                                                    int pixel = 3 * ps[0] + 4 * ps[1] + 2 * ps[2];
                                                    //pixel = (pixel >> 3);

                                                    //if we should make the destination ON
                                                    if (pixel < threshold) //use 1024 instead of 128, so that pixel does not need right shifted by 3
                                                    {
                                                        //the destination horizontal column
                                                        int destinationX = x + offsetX;

                                                        //the mask that is used to turn the single bit on
                                                        byte mask = (byte)(0x80 >> (destinationX & 0x7));

                                                        //apply the mask to the proper destination location
                                                        pd[destinationY * strideWidth + (destinationX >> 3)] |= mask;
                                                    }
                                                    ps += 3;
                                                }
                                            }
                                        }
                                        break;
                                    case MaskType.SeventyFivePercent:
                                        {
                                            //for each row in the source image
                                            for (int y = 0; y < maxY; y++)
                                            {
                                                //get the pointer to the source data on the current row
                                                byte* ps = (byte*)sourceData.Scan0 + y * sourceData.Stride;

                                                //the destination row
                                                int destinationY = y + offsetY;

                                                //for each source 'pixel'
                                                for (int x = 0; x < maxX; x++)
                                                {
                                                    int pixel = 3 * ps[0] + 4 * ps[1] + 2 * ps[2];
                                                    //pixel = (pixel >> 3);

                                                    //if we should make the destination ON
                                                    if (pixel < threshold && (y % 2 == 0 || x % 2 == 0)) //use 1024 instead of 128, so that pixel does not need right shifted by 3
                                                    {
                                                        //the destination horizontal column
                                                        int destinationX = x + offsetX;

                                                        //the mask that is used to turn the single bit on
                                                        byte mask = (byte)(0x80 >> (destinationX & 0x7));

                                                        //apply the mask to the proper destination location
                                                        pd[destinationY * strideWidth + (destinationX >> 3)] |= mask;
                                                    }
                                                    ps += 3;
                                                }
                                            }
                                        }
                                        break;
                                    case MaskType.FiftyPercent:
                                        {
                                            //for each row in the source image
                                            for (int y = 0; y < maxY; y++)
                                            {
                                                //get the pointer to the source data on the current row
                                                byte* ps = (byte*)sourceData.Scan0 + y * sourceData.Stride;

                                                //the destination row
                                                int destinationY = y + offsetY;

                                                //for each source 'pixel'
                                                for (int x = 0; x < maxX; x++)
                                                {
                                                    int pixel = 3 * ps[0] + 4 * ps[1] + 2 * ps[2];
                                                    //pixel = (pixel >> 3);

                                                    //if we should make the destination ON
                                                    if (pixel < threshold && ((y % 2 == 0 && x % 2 == 0) || (y % 2 == 1 && x % 2 == 1))) //use 1024 instead of 128, so that pixel does not need right shifted by 3
                                                    {
                                                        //the destination horizontal column
                                                        int destinationX = x + offsetX;

                                                        //the mask that is used to turn the single bit on
                                                        byte mask = (byte)(0x80 >> (destinationX & 0x7));

                                                        //apply the mask to the proper destination location
                                                        pd[destinationY * strideWidth + (destinationX >> 3)] |= mask;
                                                    }
                                                    ps += 3;
                                                }
                                            }
                                        }
                                        break;
                                    case MaskType.TwentyFivePercent:
                                        {
                                            //for each row in the source image
                                            for (int y = 0; y < maxY; y++)
                                            {
                                                //get the pointer to the source data on the current row
                                                byte* ps = (byte*)sourceData.Scan0 + y * sourceData.Stride;

                                                //the destination row
                                                int destinationY = y + offsetY;

                                                //for each source 'pixel'
                                                for (int x = 0; x < maxX; x++)
                                                {
                                                    int pixel = 3 * ps[0] + 4 * ps[1] + 2 * ps[2];
                                                    //pixel = (pixel >> 3);

                                                    //if we should make the destination ON
                                                    if (pixel < threshold && ((x % 4 == 0 && y % 2 == 0) || (x % 4 == 2 && y % 2 == 1))) //use 1024 instead of 128, so that pixel does not need right shifted by 3
                                                    {
                                                        //the destination horizontal column
                                                        int destinationX = x + offsetX;

                                                        //the mask that is used to turn the single bit on
                                                        byte mask = (byte)(0x80 >> (destinationX & 0x7));

                                                        //apply the mask to the proper destination location
                                                        pd[destinationY * strideWidth + (destinationX >> 3)] |= mask;
                                                    }
                                                    ps += 3;
                                                }
                                            }
                                        }
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException("maskType", maskType, "Unsupported mask type");
                                }
                            }
                        }
                        image.UnlockBits(sourceData);
                    }
                    break;
                case ImageType.TrueColor:
                    {
                        if (image.PixelFormat != PixelFormat.Format24bppRgb)
                        {
                            throw new InvalidOperationException(String.Format("Unsupported BitBlt source format: {0}", image.PixelFormat));
                        }

                        BitmapData sourceData = image.LockBits(new System.Drawing.Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                        int strideWidth = this.width * 3;

                        unsafe
                        {
                            fixed (byte* dst = this.data)
                            {
                                byte* pd = dst;

                                int maxY = image.Height;

                                //if the source image bottom exceeds the bounds of the destination bottom 
                                if (image.Height + offsetY > this.height)
                                {
                                    //make the maximum source height be such that the source height + y offset
                                    //goes to but does not exceed the destination's bottom
                                    maxY = this.height - offsetY;
                                }

                                int maxX = image.Width;

                                //if the source image right edge exceeds the bounds of the destination right edge 
                                if (image.Width + offsetX >= this.width)
                                {
                                    //make the maximum source right edge be such that the source right edge + x offset
                                    //goes to but does not exceed the destination's right edge
                                    maxX = this.width - offsetX;
                                }

                                //for each row in the source image
                                for (int y = 0; y < maxY; y++)
                                {
                                    //get the pointer to the source data on the current row
                                    byte* ps = (byte*)sourceData.Scan0 + y * sourceData.Stride;

                                    //the destination row
                                    int destinationY = y + offsetY;

                                    //for each source 'pixel'
                                    for (int x = 0; x < maxX; x++)
                                    {
                                        //                                        Color sourceColor = Color.FromArgb(ps[2], ps[1], ps[0]);

                                        //the destination horizontal column
                                        int destinationX = x + offsetX;
                                        int offset = destinationY * strideWidth + destinationX * 3;

                                        //System.Windows.Media.Color destinationColor = new System.Windows.Media.Color();
                                        //destinationColor.B = pd[offset + 0];
                                        //destinationColor.G = pd[offset + 1];
                                        //destinationColor.R = pd[offset + 2];

                                        //destinationColor = System.Windows.Media.Color.Add(sourceColor, destinationColor);
                                        //destinationColor = ImageData.AddColorPixels(sourceColor, destinationColor);


                                        pd[offset + 0] = ImageData.AddColor(ps[0], pd[offset + 0]);
                                        pd[offset + 1] = ImageData.AddColor(ps[1], pd[offset + 1]);
                                        pd[offset + 2] = ImageData.AddColor(ps[2], pd[offset + 2]);


                                        //System.Windows.Media.Color sourceColor = new System.Windows.Media.Color();
                                        //sourceColor.B = ps[0];
                                        //sourceColor.G = ps[1];
                                        //sourceColor.R = ps[2];

                                        ////the destination horizontal column
                                        //int destinationX = x + offsetX;
                                        //int offset = destinationY * strideWidth + destinationX * 3;

                                        //System.Windows.Media.Color destinationColor = new System.Windows.Media.Color();
                                        //destinationColor.B = pd[offset + 0];
                                        //destinationColor.G = pd[offset + 1];
                                        //destinationColor.R = pd[offset + 2];

                                        ////destinationColor = System.Windows.Media.Color.Add(sourceColor, destinationColor);
                                        //destinationColor = ImageData.AddColorPixels(sourceColor, destinationColor);

                                        //pd[offset + 0] = destinationColor.B;
                                        //pd[offset + 1] = destinationColor.G;
                                        //pd[offset + 2] = destinationColor.R;

                                        ps += 3;
                                    }
                                }
                            }
                        }
                        image.UnlockBits(sourceData);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("imageType", this.imageType, "Unsupported destination BitBlt");
            }
        }

        private static byte AddColor(byte sourceColor, byte destinationColor)
        {
            float source = (float)sourceColor / (float)255;
            float destination = (float)destinationColor / (float)255;

            float sourceAlpha = 1.0f;
            float destinationAlpha = 1.0f;

            //if the source is greater than the destination
            if (source > destination)
            {
                //use the inverse of the source as the source alpha 
                //(use more of the destination color than the source color)
                sourceAlpha = 1 - source;
                destinationAlpha = source;
            }
            else //otherwise the destination is greater than the source
            {
                //use the destination as the source alpha 
                //(use more of the source color than the destination color)
                sourceAlpha = destination;
                destinationAlpha = 1 - destination;
            }

            int result = (int)((sourceAlpha * source + destinationAlpha * destination) * 255f);

            if (result > 255)
            {
                result = 255;
            }
            if (result < 0)
            {
                result = 0;
            }

            return (byte)result;
        }

        public void InvertData()
        {
            int len = this.data.Length;
            if (len % 4 == 0)
            {
                len = len / 4;
                unsafe
                {
                    fixed (byte* bytePtr = this.data)
                    {
                        uint* uintPtr = (uint*)bytePtr;
                        for (int index = 0; index < len; ++index)
                        {
                            *uintPtr = ~*uintPtr;
                            uintPtr++;
                        }
                    }
                }

            }
            else
            {
                for (int index = 0; index < this.data.Length; ++index)
                {
                    this.data[index] = (byte)~this.data[index];
                }
            }
        }

        public ImageData CopyInvertData()
        {
            ImageData copy = new ImageData(this);
            int len = copy.data.Length;
            if (len % 4 == 0)
            {
                len = len / 4;
                unsafe
                {
                    fixed (byte* bytePtr = copy.data)
                    {
                        uint* uintPtr = (uint*)bytePtr;
                        for (int index = 0; index < len; ++index)
                        {
                            *uintPtr = ~*uintPtr;
                            uintPtr++;
                        }
                    }
                }

            }
            else
            {
                for (int index = 0; index < copy.data.Length; ++index)
                {
                    copy.data[index] = (byte)~copy.data[index];
                }
            }

            return copy;
        }

        public byte[] Data
        {
            get
            {
                return this.data;
            }
            set
            {
                this.data = value;
            }
        }
        public int Width
        {
            get
            {
                return this.width;
            }
        }

        public int Height
        {
            get
            {
                return this.height;
            }
        }

        public ImageType ImageType
        {
            get
            {
                return this.imageType;
            }
        }

        public static unsafe Bitmap Create1BppBitmap(int width, byte[] imageData)
        {
            int height = (int)((imageData.Length * 8) / width);
            return Create1BppBitmap(width, height, imageData);
        }

        public static unsafe Bitmap Create1BppBitmap(int width, int height, byte[] imageData)
        {
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format1bppIndexed);
            BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr ptr = bitmapdata.Scan0;
            byte* numPtr = (byte*)ptr;
            fixed (byte* numRef = imageData)
            {
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < (width / 8); j++)
                    {
                        numPtr[(i * bitmapdata.Stride) + j] = numRef[(i * (width / 8)) + j];
                    }
                }
            }
            bitmap.UnlockBits(bitmapdata);
            return bitmap;
        }
    }
}
