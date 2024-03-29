﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Docking;
using Visualizer.ViewModel;

namespace Visualizer.Util
{
    public class CustomDockingPanesFactory : DockingPanesFactory
    {
        private PaneViewModel currentPaneViewModel;

        protected override void AddPane(RadDocking radDocking, RadPane pane)
        {
            var paneModel = currentPaneViewModel;

            if (paneModel != null && !(paneModel.IsDocument))
            {
                RadPaneGroup group = null;
                switch (paneModel.InitialPosition)
                {
                    case DockState.DockedRight:
                        group = radDocking.SplitItems.ToList().FirstOrDefault(i => i.Control.Name == "rightGroup") as RadPaneGroup;
                        if (group != null)
                        {
                            group.Items.Add(pane);
                        }
                        return;
                    case DockState.DockedBottom:
                        group = radDocking.SplitItems.ToList().FirstOrDefault(i => i.Control.Name == "bottomGroup") as RadPaneGroup;
                        if (group != null)
                        {
                            group.Items.Add(pane);
                        }
                        return;
                    case DockState.DockedLeft:
                        group = radDocking.SplitItems.ToList().FirstOrDefault(i => i.Control.Name == "leftGroup") as RadPaneGroup;
                        if (group != null)
                        {
                            group.Items.Add(pane);
                        }
                        return;
                    case DockState.FloatingDockable:
                        var fdSplitContainer = radDocking.GeneratedItemsFactory.CreateSplitContainer();
                        group = radDocking.GeneratedItemsFactory.CreatePaneGroup();
                        fdSplitContainer.Items.Add(group);
                        group.Items.Add(pane);
                        radDocking.Items.Add(fdSplitContainer);
                        pane.MakeFloatingDockable();
                        return;
                    case DockState.FloatingOnly:
                        var foSplitContainer = radDocking.GeneratedItemsFactory.CreateSplitContainer();
                        group = radDocking.GeneratedItemsFactory.CreatePaneGroup();
                        foSplitContainer.Items.Add(group);
                        group.Items.Add(pane);
                        radDocking.Items.Add(foSplitContainer);
                        pane.MakeFloatingOnly();
                        return;
                    case DockState.DockedTop:
                    default:
                        return;
                }
            }

            base.AddPane(radDocking, pane);
        }

        protected override RadPane CreatePaneForItem(object item)
        {
            var viewModel = item as PaneViewModel;
            if (viewModel != null)
            {
                var pane = viewModel.IsDocument ? new RadDocumentPane() : new RadPane();
                pane.DataContext = item;
                RadDocking.SetSerializationTag(pane, viewModel.Header);
                if (viewModel.ContentType != null)
                {
                    object vm = Activator.CreateInstance(viewModel.ContentType, new object[] { });
                    pane.Content = vm;
                    pane.DataContext = vm;
                }

                currentPaneViewModel = viewModel;
                pane.Header = viewModel.Header;
                return pane;
            }

            return base.CreatePaneForItem(item);
        }

        protected override void RemovePane(RadPane pane)
        {
            if (pane != null)
            {
                pane.DataContext = null;
                pane.Content = null;
                pane.ClearValue(RadDocking.SerializationTagProperty);
                pane.RemoveFromParent();
            }
        }
    }
}
