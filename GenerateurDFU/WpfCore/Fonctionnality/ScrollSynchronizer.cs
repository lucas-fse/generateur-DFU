using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
//using System.Workflow.ComponentModel;

namespace JAY.WpfCore
{
    public class ScrollSynchronizer : DependencyObject
    {
        #region Variables

        private static Dictionary<ScrollViewer, string> _scrollViewers = new Dictionary<ScrollViewer, string>();
        private static Dictionary<string, double> _horizontalScrollOffsets = new Dictionary<string, double>();
        private static Dictionary<string, double> _verticalScrollOffsets = new Dictionary<string, double>();

        #endregion

        // Définition de la propriété de dépendance ScrollGroup
        public static readonly DependencyProperty ScrollGroupProperty =
        DependencyProperty.RegisterAttached(
        "ScrollGroup",
        typeof(string),
        typeof(ScrollSynchronizer),
        new PropertyMetadata(new PropertyChangedCallback(OnScrollGroupChanged)));

        public static void SetScrollGroup(DependencyObject obj, string scrollGroup)
        {
            obj.SetValue(ScrollGroupProperty, scrollGroup);
        }

        public static string GetScrollGroup(DependencyObject obj)
        {
            return (string)obj.GetValue(ScrollGroupProperty);
        }

        // Définition de la méthode liée au changement de la propriété ScrollGroup

        /// <summary>
        /// Traitement du CallBack lorsque le groupe de scroll change
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnScrollGroupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var scrollViewer = d as ScrollViewer;
            if (scrollViewer != null)
            {
                if (!string.IsNullOrEmpty((string)e.OldValue))
                {
                    // Remove scrollviewer
                    if (_scrollViewers.ContainsKey(scrollViewer))
                    {
                        scrollViewer.ScrollChanged -= new ScrollChangedEventHandler(ScrollViewer_ScrollChanged);
                        _scrollViewers.Remove(scrollViewer);
                    }
                }

                if (!string.IsNullOrEmpty((string)e.NewValue))
                {
                    // If group already exists, set scrollposition of 
                    // new scrollviewer to the scrollposition of the group
                    if (_horizontalScrollOffsets.Keys.Contains((string)e.NewValue))
                    {
                        scrollViewer.ScrollToHorizontalOffset(_horizontalScrollOffsets[(string)e.NewValue]);
                    }
                    else
                    {
                        _horizontalScrollOffsets.Add((string)e.NewValue, scrollViewer.HorizontalOffset);
                    }

                    if (_verticalScrollOffsets.Keys.Contains((string)e.NewValue))
                    {
                        scrollViewer.ScrollToVerticalOffset(_verticalScrollOffsets[(string)e.NewValue]);
                    }
                    else
                    {
                        _verticalScrollOffsets.Add((string)e.NewValue, scrollViewer.VerticalOffset);
                    }

                    // Add scrollviewer
                    _scrollViewers.Add(scrollViewer, (string)e.NewValue);
                    scrollViewer.ScrollChanged += new ScrollChangedEventHandler(ScrollViewer_ScrollChanged);
                }
            }
        }

        /// <summary>
        /// Traitement de l'évenement ScrollChanged pour chacun des ScrollViewer du groupe concerné
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalChange != 0 || e.HorizontalChange != 0)
            {
                var changedScrollViewer = sender as ScrollViewer;
                Scroll(changedScrollViewer);
            }
        }

        /// <summary>
        /// Appliquer les éléments de Scroll pour les deux directions pour le ScrollViewer spécifié
        /// </summary>
        /// <param name="changedScrollViewer"></param>
        private static void Scroll(ScrollViewer changedScrollViewer)
        {
            var group = _scrollViewers[changedScrollViewer];
            _verticalScrollOffsets[group] = changedScrollViewer.VerticalOffset;
            _horizontalScrollOffsets[group] = changedScrollViewer.HorizontalOffset;

            foreach (var scrollViewer in _scrollViewers.Where((s) => s.Value == group && s.Key != changedScrollViewer))
            {
                if (scrollViewer.Key.VerticalOffset != changedScrollViewer.VerticalOffset)
                {
                    scrollViewer.Key.ScrollToVerticalOffset(changedScrollViewer.VerticalOffset);
                }

                if (scrollViewer.Key.HorizontalOffset != changedScrollViewer.HorizontalOffset)
                {
                    scrollViewer.Key.ScrollToHorizontalOffset(changedScrollViewer.HorizontalOffset);
                }
            }
        }
    }
}
