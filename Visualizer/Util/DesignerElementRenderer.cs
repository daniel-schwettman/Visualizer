using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Visualizer.Util
{
    public class DesignerElementRenderer : IDesignerElementRenderer
    {
        private IDesignerElement _element;
        private object m_lock = new object();
        private readonly string Name = "DesignerElementRenderer";

#if SAVE_PDF_IMAGES
        private static int imageCount;
#endif

        public DesignerElementRenderer(IDesignerElement viewModelElement)
        {
            this._element = viewModelElement;

            this._element.PreviewRender();
        }

        public Rect GetElementBounds(Matrix renderingMatrix)
        {
            //transform the element's transformed bounds (translate and rotate) by the matrix transform of the target surface
            MatrixTransform matrixTransform = TransformHelper.CreateMatrixTransform(renderingMatrix);
            return matrixTransform.TransformBounds(this._element.TransformedBounds);
        }

//        public BitmapSource Render(ImageData targetImage, Matrix renderingMatrix, int bitmapWidth, int bitmapHeight, int offsetX, int offsetY, MaskType mask, bool isVerticalSwaths)
//        {
//            // Logger.AddLog(LogType.Diag, this.Name, string.Format("Render: BitmapWidth={0}, BitmapHeight={1}, OffsetX={2}, OffsetY={3}", bitmapWidth, bitmapHeight, offsetX, offsetY));

//            if (this._element.IsDpiDependent == false)
//            {
//                System.Diagnostics.Stopwatch watchTotal = new System.Diagnostics.Stopwatch();
//                System.Diagnostics.Stopwatch watchRenderCore = new System.Diagnostics.Stopwatch();
//                System.Diagnostics.Stopwatch watchBitBlt = new System.Diagnostics.Stopwatch();
//                System.Diagnostics.Stopwatch watchAquire = new System.Diagnostics.Stopwatch();
//                System.Diagnostics.Stopwatch watchRender = new System.Diagnostics.Stopwatch();
//                watchTotal.Start();

//                watchAquire.Start();
//                //create the rendering bitmap for the drawable item. Its size is only the size of the drawable's intersection with the head image
//                //BitmapSource elementImage = new RenderTargetBitmap(bitmapWidth, bitmapHeight, 96d, 96d, PixelFormats.Pbgra32);
//                BitmapSource elementImage = WpfRenderTargetBitmapCacheHelper.Aquire(bitmapWidth, bitmapHeight);
//                //Logger.AddLog(LogType.Diag, this.Name, $"Target (canvas) image width={targetImage.Width}, height={targetImage.Height}. imageType={targetImage.ImageType}");
//                //Logger.AddLog(LogType.Diag, this.Name, $"Element image bitmapWidth={bitmapWidth}, bitmapHeight={bitmapHeight}");
//                watchAquire.Stop();
//#if TEST
//                DrawingGroup drawingGroup = new DrawingGroup();
//                RenderOptions.SetEdgeMode(drawingGroup, EdgeMode.Aliased);
//                using (DrawingContext dc = drawingGroup.Open())
//                {
//                    dc.DrawRectangle(Brushes.White, null, new Rect(0, 0, bitmapWidth, bitmapHeight));
//                    dc.PushTransform(TransformHelper.CreateMatrixTransform(renderingMatrix));
//                    //dc.PushTransform(new ScaleTransform(6d, 6d));
//                    //if (this._element.Rotation != 0)
//                    //{
//                    //    //apply rotation transform before rendering element
//                    //    dc.PushTransform(TransformHelper.CreateRotateTransform(this._element.Rotation, this._element.CenterX, this._element.CenterY));
//                    //}
//                    this._element.RenderCore(dc);
//                }

//                //DrawingVisual dvx = new DrawingVisual();
//                //using (DrawingContext dc = dvx.RenderOpen())
//                //{
//                //    //dc.DrawRectangle(Brushes.White, null, new Rect(0, 0, bitmapWidth, bitmapHeight));
//                //    //dc.PushTransform(TransformHelper.CreateMatrixTransform(renderingMatrix));
//                //    if (this._element.Rotation != 0)
//                //    {
//                //        //apply rotation transform before rendering element
//                //        dc.PushTransform(TransformHelper.CreateRotateTransform(this._element.Rotation, this._element.CenterX, this._element.CenterY));
//                //    }
//                //    this._element.RenderCore(dc);
//                //}

//                //string text = System.IO.File.ReadAllText("offset.txt");
//                //double offset = Double.Parse(text);
//                //DrawingGroup dg = new DrawingGroup();
//                //RenderOptions.SetEdgeMode(dg, EdgeMode.Aliased);
//                //using (DrawingContext dc = dg.Open())
//                //{
//                //    dc.DrawRectangle(new VisualBrush(dvx), null, new Rect(offset, 0, bitmapWidth, bitmapHeight));
//                //    dc.d
//                //    //dc.DrawImage((new DrawingImage(dvx), null, new Rect(offset, 0, bitmapWidth, bitmapHeight));
//                //}

//                DrawingVisual dv = new DrawingVisual();
//                using (DrawingContext dc = dv.RenderOpen())
//                {
//                    //dc.DrawRectangle(Brushes.White, null, new Rect(0, 0, bitmapWidth, bitmapHeight));
//                    //dc.PushTransform(new ScaleTransform(10d, 10d));
//                    dc.DrawDrawing(drawingGroup);
//                }

//                //DrawingVisual dv = new DrawingVisual();
//                //using (DrawingContext dc = dv.RenderOpen())
//                //{
//                //    dc.DrawRectangle(Brushes.White, null, new Rect(0, 0, bitmapWidth, bitmapHeight));
//                //    dc.PushTransform(TransformHelper.CreateMatrixTransform(renderingMatrix));
//                //    //GuidelineSet set = new GuidelineSet();
//                //    //set.GuidelinesX.Add(0d);
//                //    //set.GuidelinesX.Add(bitmapWidth);
//                //    //set.GuidelinesY.Add(0d);
//                //    //set.GuidelinesY.Add(bitmapHeight);

//                //    //dc.PushGuidelineSet(set);
//                //    if (this._element.Rotation != 0)
//                //    {
//                //        //apply rotation transform before rendering element
//                //        dc.PushTransform(TransformHelper.CreateRotateTransform(this._element.Rotation, this._element.CenterX, this._element.CenterY));
//                //    }

//                //    this._element.RenderCore(dc);
//                //}

//                //RenderOptions.SetEdgeMode(dv.Drawing, EdgeMode.Aliased);
//                //RenderOptions.SetEdgeMode(dv, EdgeMode.Aliased);
//#endif
//                watchRenderCore.Start();

//                DrawingVisual dv = new DrawingVisual();
//                using (DrawingContext dc = dv.RenderOpen())
//                {
//                    dc.DrawRectangle(Brushes.White, null, new Rect(0, 0, bitmapWidth, bitmapHeight));
//                    dc.PushTransform(TransformHelper.CreateMatrixTransform(renderingMatrix));
//                    if (this._element.Rotation != 0)
//                    {
//                        //apply rotation transform before rendering element
//                        dc.PushTransform(TransformHelper.CreateRotateTransform(this._element.Rotation, this._element.CenterX, this._element.CenterY));
//                    }

//                    this._element.RenderCore(dc);
//                }
//                watchRenderCore.Stop();

//                watchRender.Start();
//                ((RenderTargetBitmap)elementImage).Render(dv);
//                watchRender.Stop();


//                // Logger.AddLog(LogType.Diag, this.Name, string.Format("Time to render element image was {0}ms", watch2.ElapsedMilliseconds));

//                //ImageData.SaveBitmap(elementImage, "rtb.bmp");
//#if SAVE_PDF_IMAGES
//				lock (m_lock)
//				{
//					BitmapSource copyImage = elementImage.Clone();
//					ImageData.SaveBitmap(copyImage, string.Format("C:\\Temp\\nondpidep_{0}.bmp", imageCount++));
//				}
//#endif

//                if (targetImage.ImageType == ImageType.Abgr)
//                {
//#warning TODO Filtering
//                    watchBitBlt.Start();

//                    bool isMonochrome = this._element.IsRGBToMonochromeEnabled;
//                    targetImage.BitBlt32bgraToArgb(elementImage, offsetX, offsetY, isMonochrome);

//                    watchBitBlt.Stop();
//                }
//                else //otherwise target is likely 1bpp
//                {
//                    FilterInfo filterInfo = this._element.GetPostProcessingFilter();

//                    //if there is filtering, apply the filter then bitblt
//                    if (filterInfo.FilterType != FilterType.None)
//                    {
//                        watchBitBlt.Start();

//                        ImageData imageData = new ImageData(elementImage);
//                        imageData.ApplyFilter(filterInfo);
//                        targetImage.BitBlt(imageData, offsetX, offsetY, mask, this._element.BitBltThreshold, this._element.IsWhiteMask);

//                        watchBitBlt.Stop();

//                        // Logger.AddLog(LogType.Diag, this.Name, string.Format("Time to filter and bitblt image was {0}ms", watch3.ElapsedMilliseconds));
//                    }
//                    else //otherwise there is no filtering so just bitblt the source Pbgra image (RenderTargetBitmap)
//                    {
//                        watchBitBlt.Start();

//                        targetImage.BitBlt(elementImage, offsetX, offsetY, mask, this._element.BitBltThreshold, this._element.IsWhiteMask);

//                        watchBitBlt.Stop();

//                        // Logger.AddLog(LogType.Diag, this.Name, string.Format("Time to bitblt image was {0}ms", watch3.ElapsedMilliseconds));

//                        //System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
//                        //watch.Start();

//                        //byte[] pixels = new byte[elementImage.PixelWidth * elementImage.PixelHeight];
//                        //elementImage.CopyPixels(pixels, elementImage.PixelWidth, 0);

//                        //MonochromeBitmap sourceBitmap = new MonochromeBitmap(pixels, (int)elementImage.Width, (int)elementImage.Height, elementImage.PixelWidth);

//                        //MonochromeBitmap destBitmap = sourceBitmap.ExtractRegion(offsetX, offsetY, targetImage.Width, targetImage.Height);
//                        //// destBitmap.InvertColors();

//                        //// targetImage.Data = destBitmap.GetBuffer();
//                        //Buffer.BlockCopy(destBitmap.GetBuffer(), 0, targetImage.Data, 0, Math.Min(targetImage.Data.Length, destBitmap.GetBuffer().Length));

//                        //watch.Stop();

//                        //destBitmap.Save(string.Format("C:\\Temp\\monoDestBitmap_{0}.bmp", imageCount++));
//                        //Logger.AddLog(LogType.Diag, this.Name, string.Format("Time to extract monochrome image was {0}ms", watch.ElapsedMilliseconds));
//                    }
//                }

//#if SAVE_PDF_IMAGES
//				lock (m_lock)
//				{
//					ImageData copy = targetImage.Clone();
//					////copy.InvertData();
//					ImageData.SaveBitmap(copy.CreateBitmapSource(), string.Format("C:\\Temp\\targetImageNonDpiDependent_{0}.bmp", imageCount++));
//				}
//#endif
//                watchTotal.Stop();
//                return elementImage;
//            }
//            else
//            {
//                double extraRotation = 0d;
//                if (isVerticalSwaths == true)
//                {
//                    extraRotation = 90d;
//                }
//                //if head is inverted, rotate element 180 degrees
//                bool inverted = renderingMatrix.M11 < 0 && renderingMatrix.M22 < 0;
//                if (inverted)
//                {
//                    extraRotation = 180;
//                }

//                int pdfWidth;
//                int pdfHeight;
//                int stride;
//                int rotation = (int)(this._element.Rotation + extraRotation);

//                // Rotation parameter not being used right now because Apago rotates the first page but not any others
//                // So we still do the actual rotation in code below
//                //
//                byte[] imagePixels = this._element.RenderDpiDependent(rotation, targetImage.ImageType, out pdfWidth, out pdfHeight, out stride);
//                //BitmapSource elementImage = this._element.RenderDpiDependent(); // targetImage, offsetX, offsetY, (int)this._element.Rotation, isVerticalSwaths);
//                BitmapSource elementImage = null;
//                KirkRudyInc.FormDesigner.DesignerElements.FormDesignerElementViewModel element = null;

//                // Logger.AddLog(LogType.Diag, this.Name, string.Format("Rotation={0}, PdfWidth={1}, PdfHeight={2}, Stride={3}", rotation, pdfWidth, pdfHeight, stride));

//                if (imagePixels != null)
//                {
//                    if (this._element is KirkRudyInc.FormDesigner.DesignerElements.FormDesignerElementViewModel)
//                    {
//                        element = (KirkRudyInc.FormDesigner.DesignerElements.FormDesignerElementViewModel)this._element;
//                    }

//                    try
//                    {
//                        double horizontalRes = 0.0;
//                        double verticalRes = 0.0;

//                        if (this._element.Rotation + extraRotation == 90 || this._element.Rotation + extraRotation == 270)
//                        {
//                            horizontalRes = element.RasterizationVerticalResolution;
//                            verticalRes = element.RasterizationHorizontalResolution;
//                        }
//                        else
//                        {
//                            horizontalRes = element.RasterizationHorizontalResolution;
//                            verticalRes = element.RasterizationVerticalResolution;
//                        }

//                        // Target imageType for KolorJet is Abgr
//                        if (targetImage.ImageType == ImageType.Abgr)
//                        {
//                            elementImage = BitmapSource.Create(pdfWidth, pdfHeight, horizontalRes, verticalRes, PixelFormats.Rgb24, BitmapPalettes.Halftone256, imagePixels, stride);

//                            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
//                            watch.Start();

//                            FormatConvertedBitmap convertedBitmap = new FormatConvertedBitmap(elementImage, PixelFormats.Bgra32, BitmapPalettes.Halftone256, 0.0);
//                            elementImage = convertedBitmap;

//                            watch.Stop();
//                            Logger.AddLog(LogType.Diag, this.Name, "Time to convert RGB to Bgra32 is {0}ms", watch.ElapsedMilliseconds);
//                        }
//                        else if (targetImage.ImageType == ImageType.Cmyk)
//                        {
//                            elementImage = BitmapSource.Create(pdfWidth, pdfHeight, horizontalRes, verticalRes, PixelFormats.Cmyk32, BitmapPalettes.Halftone256, imagePixels, stride);
//                        }
//                        else
//                        {
//                            elementImage = BitmapSource.Create(pdfWidth, pdfHeight, horizontalRes, verticalRes, PixelFormats.BlackWhite, BitmapPalettes.BlackAndWhite, imagePixels, stride);
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        Logger.AddLog(LogType.Diag, this.Name, string.Format("Rip Failed to create bitmap. Error: {0}", ex.Message));
//                    }
//                }
//                else
//                {
//                    Logger.AddLog(LogType.Diag, this.Name, "Rip Error: No image returned from rip");
//                }

//                if (elementImage != null)
//                {
//#if SAVE_PDF_IMAGES
//					lock (m_lock)
//					{
//						BitmapSource copyImage = elementImage.Clone();
//						ImageData.SaveBitmap(copyImage, string.Format("C:\\Temp\\dpidep_{0}.bmp", imageCount++));
//					}
//#endif
//                    if (inverted == true || isVerticalSwaths == true || this._element.Rotation != 0)
//                    {
//                        elementImage = new TransformedBitmap(elementImage, new RotateTransform(this._element.Rotation + extraRotation));
//                        elementImage.Freeze();

//#if SAVE_PDF_IMAGES
//                        lock (m_lock)
//                        {
//                            BitmapSource copyImage = elementImage.Clone();
//                            ImageData.SaveBitmap(copyImage, string.Format("C:\\Temp\\dpidep_rotated_{0}.bmp", imageCount++));
//                        }
//#endif
//                    }

//                    double newOffsetX = 0 - renderingMatrix.OffsetX;
//                    double newOffsetY = 0 - renderingMatrix.OffsetY;

//                    //renderingMatrix.OffsetX = 0;
//                    //renderingMatrix.OffsetY = 0;
//                    //elementImage = new TransformedBitmap(elementImage, TransformHelper.CreateMatrixTransform(renderingMatrix));

//                    //// elementImage = new ColorConvertedBitmap(elementImage, new ColorContext(PixelFormats.Gray8), new ColorContext(PixelFormats.BlackWhite), PixelFormats.BlackWhite);

//                    //lock (m_lock)
//                    //{
//                    //	BitmapSource copyImage = elementImage.Clone();
//                    //	ImageData.SaveBitmap(copyImage, string.Format("C:\\Temp\\dpidep_transformed_{0}.bmp", imageCount++));
//                    //}

//                    //// Create an Image control
//                    //System.Windows.Controls.Image bwImage = new System.Windows.Controls.Image();

//                    //// Create a new image using FormatConvertedBitmap and set DestinationFormat to GrayScale
//                    //FormatConvertedBitmap bwBitmap = new FormatConvertedBitmap();
//                    //bwBitmap.BeginInit();
//                    //bwBitmap.Source = elementImage;
//                    //bwBitmap.DestinationFormat = PixelFormats.BlackWhite;
//                    //bwBitmap.EndInit();

//                    // Set Source property of Image

//                    // bwImage.Source = bwBitmap;

//                    // Default target type for KolorJet is Abgr
//                    // New experimental target type is Cmyk
//                    if (targetImage.ImageType == ImageType.Abgr)
//                    {
//#warning TODO Filtering
//                        System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
//                        watch.Start();

//                        bool isMonochrome = this._element.IsRGBToMonochromeEnabled;
//                        targetImage.BitBlt32bgraToArgb(elementImage, offsetX, offsetY, isMonochrome);

//                        watch.Stop();
//                        Logger.AddLog(LogType.Diag, this.Name, string.Format("Time to bitblt color image was {0}ms", watch.ElapsedMilliseconds));
//                    }
//                    else if (targetImage.ImageType == ImageType.Cmyk)
//                    {
//                        System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
//                        watch.Start();

//                        int newStride = targetImage.Width * 4;
//                        CroppedBitmap croppedBitmap = new CroppedBitmap(elementImage, new Int32Rect(offsetX, offsetY, targetImage.Width, targetImage.Height));
//                        croppedBitmap.CopyPixels(targetImage.Data, newStride, 0);

//                        watch.Stop();
//                        Logger.AddLog(LogType.Diag, this.Name, string.Format("Time to crop color image was {0}ms", watch.ElapsedMilliseconds));

//#if SAVE_PDF_IMAGES
//                        lock (m_lock)
//                        {
//                            croppedBitmap.Save(string.Format("C:\\Temp\\dpidep_cropped_{0}.bmp", imageCount++));
//                        }
//#endif
//                    }
//                    else
//                    {
//                        if (elementImage.Format != PixelFormats.BlackWhite)
//                        {
//                            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
//                            watch.Start();

//                            targetImage.BitBlt(elementImage, offsetX, offsetY, mask, this._element.BitBltThreshold, this._element.IsWhiteMask);

//                            watch.Stop();

//                            // Logger.AddLog(LogType.Diag, this.Name, string.Format("Time to bitblt image was {0}ms", watch.ElapsedMilliseconds));
//                        }
//                        else
//                        {
//                            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
//                            watch.Start();

//                            //int height = bwBitmap.PixelHeight;
//                            //int width = bwBitmap.PixelWidth;
//                            //int bwStride = width * ((bwBitmap.Format.BitsPerPixel + 7) / 8);

//                            //byte[] bits = new byte[height * bwStride];
//                            //bwBitmap.CopyPixels(bits, bwStride, 0);
//                            MonochromeBitmap sourceBitmap = new MonochromeBitmap(imagePixels, pdfWidth, pdfHeight, stride);

//                            if (inverted == true || isVerticalSwaths == true || this._element.Rotation != 0)
//                            {
//                                if (this._element.Rotation + extraRotation == 90)
//                                {
//                                    sourceBitmap.Rotate90();
//                                }
//                                else if (this._element.Rotation + extraRotation == 180)
//                                {
//                                    sourceBitmap.Rotate180();
//                                }
//                                else if (this._element.Rotation + extraRotation == 270)
//                                {
//                                    sourceBitmap.Rotate270();
//                                }
//                            }

//                            // Logger.AddLog(LogType.Diag, this.Name, string.Format("Source Bitmap: Width={0}, Height={1}, WidthInBytes={2}", sourceBitmap.Width, sourceBitmap.Height, sourceBitmap.WidthInBytes));

//                            System.Runtime.InteropServices.GCHandle gchDest = System.Runtime.InteropServices.GCHandle.Alloc(targetImage.Data, System.Runtime.InteropServices.GCHandleType.Pinned);

//                            int widthInBytes = ((targetImage.Width + 7) / 8);
//                            IntPtr ptr = gchDest.AddrOfPinnedObject(); // System.Runtime.InteropServices.GCHandle.ToIntPtr(gch);

//                            unsafe
//                            {
//                                MonochromeBitmap destBitmap = new MonochromeBitmap(&ptr, targetImage.Width, targetImage.Height, widthInBytes);
//                                // MonochromeBitmap destBitmap = new MonochromeBitmap(targetImage.Width, targetImage.Height);
//                                // Logger.AddLog(LogType.Diag, this.Name, string.Format("Dest Bitmap: Width={0}, Height={1}, WidthInBytes={2}", targetImage.Width, targetImage.Height, widthInBytes));

//                                destBitmap.InvertColors();

//                                int sourceWidth = Math.Min(sourceBitmap.Width - (int)newOffsetX, targetImage.Width);
//                                int sourceHeight = Math.Min(sourceBitmap.Height - (int)newOffsetY, targetImage.Height);

//                                // Logger.AddLog(LogType.Diag, this.Name, string.Format("Copy Region: OffsetX={0}, OffsetY={1}, SourceWidth={2}, SourceHeight={3}", newOffsetX, newOffsetY, sourceWidth, sourceHeight));

//                                destBitmap.CopyRegion(sourceBitmap, (int)newOffsetX, (int)newOffsetY, sourceWidth, sourceHeight, 0, 0);
//                                destBitmap.InvertColors();

//#if SAVE_PDF_IMAGES
//								destBitmap.Save(string.Format("C:\\Temp\\monoDestBitmap_{0}.bmp", imageCount++));
//#endif
//                            }
//                            //if (additionalRotation == -90d)
//                            //{
//                            //	destBitmap.Rotate270();
//                            //}

//                            //if (destBitmap.Width < targetImage.Width || destBitmap.Height < targetImage.Height)
//                            //{
//                            //	for (int i = 0; i < Math.Min(destBitmap.Height, targetImage.Height); i++)
//                            //	{
//                            //		int sourceStart = i * destBitmap.WidthInBytes;
//                            //		int destinationStart = i * targetImage.Width / 8;
//                            //		int copyLength = Math.Min(destBitmap.WidthInBytes, targetImage.Width / 8);
//                            //		Buffer.BlockCopy(destBitmap.GetBuffer(), sourceStart, targetImage.Data, destinationStart, copyLength);
//                            //	}
//                            //}
//                            //else
//                            //{
//                            // targetImage.Data = destBitmap.GetBuffer();
//                            // int destinationLength = destBitmap.GetBuffer().Length;
//                            // Buffer.BlockCopy(destBitmap.GetBuffer(), 0, targetImage.Data, 0, Math.Min(targetImage.Data.Length, destinationLength));
//                            //}

//                            gchDest.Free();
//                            sourceBitmap.Dispose();

//                            watch.Stop();

//                            // Logger.AddLog(LogType.Diag, this.Name, string.Format("Time to extract monochrome image was {0}ms", watch.ElapsedMilliseconds));
//                        }
//                    }

//                    //Logger.AddLog(LogType.Diag, this.Name, string.Format("Mask is: {0}. IsWhiteMask: {1}, BitBltThreshold: {2}", mask, this._element.IsWhiteMask, this._element.BitBltThreshold));
//                    //ImageData.SaveBitmap(elementImage, string.Format("C:\\Temp\\elementImageDpiDependent_{0}.bmp", imageCount++));

//#if SAVE_PDF_IMAGES
//					lock (m_lock)
//					{
//						ImageData copy = targetImage.Clone();
//						ImageData.SaveBitmap(copy.CreateBitmapSource(), string.Format("C:\\Temp\\targetImageDpiDependent_{0}.bmp", imageCount++));
//					}
//#endif
//                }

//                return elementImage;
//            }
//        }

        public void Dispose()
        {
            this._element.RenderDispose();
        }

        public BitmapSource Render(ImageData targetImage, Matrix renderingMatrix, int bitmapWidth, int bitmapHeight, int offsetX, int offsetY, MaskType mask, bool isVerticalSwaths)
        {
            throw new NotImplementedException();
        }
    }
}
