using System;
using System.Drawing;
using System.Globalization;
using Bumblebee.Interfaces;
using Bumblebee.Setup;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Interactions.Internal;
using OpenQA.Selenium.Support.UI;

namespace Bumblebee.Extensions
{
    public static class AdvancedSeleniumActions
    {
        public static void ClickLocation(this Session session, int x, int y)
        {
            var body = session.Driver.GetElement("html");
            new Actions(session.Driver).MoveToElement(body, x, y).Click().Build().Perform();
        }

        public static TElement Hover<TElement>(this TElement element, int miliseconds = 0) where TElement : IElement
        {
            new Actions(element.Session.Driver).MoveToElement(element.Tag).Perform();

            return element.Pause(miliseconds);
        }

        public static DragAction<TParent> Drag<TParent>(this TParent parent, Func<TParent, IDraggable> getDraggable) where TParent : IBlock
        {
            return new DragAction<TParent>(parent, getDraggable);
        }

        public static TParent WaitUntil<TParent>(this TParent parent, Predicate<TParent> condition, int miliseconds = 10000) where TParent : IBlock
        {
            var wait = new DefaultWait<TParent>(parent) {Timeout = TimeSpan.FromMilliseconds(miliseconds)};
            wait.Until(condition.Invoke);
            return parent;
        }
    }

    public class DragAction<TParent> where TParent : IBlock
    {
        private TParent Parent { get; set; }
        private IDraggable Draggable { get; set; }

        public DragAction(TParent parent, Func<TParent, IDraggable> getDraggable)
        {
            Parent = parent;
            Draggable = getDraggable(parent);
        }

        public TParent AndDrop(Func<TParent, IHasBackingElement> getDropzone)
        {
            PerformDragAndDrop(getDropzone);

            return Parent.Session.CurrentBlock<TParent>();
        }

        public TCustomResult AndDrop<TCustomResult>(Func<TParent, IHasBackingElement> getDropzone) where TCustomResult : IBlock
        {
            PerformDragAndDrop(getDropzone);

            return Parent.Session.CurrentBlock<TCustomResult>();
        }

        public TParent AndDrop(int xOffset, int yOffset)
        {
            PerformDragAndDrop(xOffset, yOffset);

            return Parent.Session.CurrentBlock<TParent>();
        }

        public TCustomResult AndDrop<TCustomResult>(int xOffset, int yOffset) where TCustomResult : IBlock
        {
            PerformDragAndDrop(xOffset, yOffset);

            return Parent.Session.CurrentBlock<TCustomResult>();
        }

        private void PerformDragAndDrop(Func<TParent, IHasBackingElement> getDropzone)
        {
            var dropzone = getDropzone(Parent);

            Parent.GetDragAndDropPerformer().DragAndDrop(Draggable.Tag, dropzone.Tag);
        }

        private void PerformDragAndDrop(int xOffset, int yOffset)
        {
            Parent.GetDragAndDropPerformer().DragAndDrop(Draggable.Tag, xOffset, yOffset);
        }
    }
}