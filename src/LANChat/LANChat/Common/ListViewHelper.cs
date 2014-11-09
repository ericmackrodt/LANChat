using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LANChat.Common
{
    // Code from: https://social.msdn.microsoft.com/Forums/vstudio/en-US/0f524459-b14e-4f9a-8264-267953418a2d/trivial-listboxlistview-autoscroll
    public static class ListViewHelper
    {
        public static readonly DependencyProperty AutoScrollProperty =
            DependencyProperty.RegisterAttached("AutoScroll", typeof(bool), typeof(ListView),
            new PropertyMetadata(false));

        public static readonly DependencyProperty AutoScrollHandlerProperty =
            DependencyProperty.RegisterAttached("AutoScrollHandler", typeof(AutoScrollHandler), typeof(ListView));

        public static bool GetAutoScroll(ListView instance)
        {
            return (bool)instance.GetValue(AutoScrollProperty);
        }

        public static void SetAutoScroll(ListView instance, bool value)
        {
            if (instance == null) return;

            AutoScrollHandler OldHandler = (AutoScrollHandler)instance.GetValue(AutoScrollHandlerProperty);
            if (OldHandler != null)
            {
                OldHandler.Dispose();
                instance.SetValue(AutoScrollHandlerProperty, null);
            }
            instance.SetValue(AutoScrollProperty, value);
            if (value)
                instance.SetValue(AutoScrollHandlerProperty, new AutoScrollHandler(instance));
        }

    }  
}
