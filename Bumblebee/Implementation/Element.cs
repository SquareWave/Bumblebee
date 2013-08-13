using Bumblebee.Extensions;
using Bumblebee.Interfaces;
using OpenQA.Selenium;

namespace Bumblebee.Implementation
{
    public abstract class Element : SpecificBlock
    {
        public IBlock ParentBlock { get; private set; }

        protected Element(IBlock parent, string cssSelector) : this(parent, By.CssSelector(cssSelector))
        {
        }

        protected Element(IBlock parent, By by) : this(parent, parent.Tag.GetElement(by))
        {
        }

        protected Element(IBlock parent, IWebElement tag) : base(parent.Session, tag)
        {
            ParentBlock = parent;
        }

        public virtual string Text
        {
            get { return Tag.Text; }
        }

        public virtual bool Selected
        {
            get { return Tag.Selected; }
        }
    }
}
