﻿using System.Collections.Generic;
using System.Linq;
using Bumblebee.Extensions;
using Bumblebee.Interfaces;
using Bumblebee.Setup;
using OpenQA.Selenium;

namespace Bumblebee.Implementation
{
    public class Clickable : Element, IClickable
    {
        public Clickable(IBlock parent, string cssSelector) 
            : base(parent, cssSelector)
        {
        }

        public Clickable(IBlock parent, By by)
            : base(parent, by)
        {
        }

        public Clickable(IBlock parent, IWebElement element)
            : base(parent, element)
        {
        }

        public virtual TResult Click<TResult>() where TResult : IBlock
        {
            Tag.Click();
            return Session.CurrentBlock<TResult>(ParentBlock.Tag);
        }
    }

    public class Clickable<TResult> : Clickable, IClickable<TResult> where TResult : IBlock
    {
        public Clickable(IBlock parent, string cssSelector)
            : base(parent, cssSelector)
        {
        }

        public Clickable(IBlock parent, By by)
            : base(parent, by)
        {
        }

        public Clickable(IBlock parent, IWebElement element)
            : base(parent, element)
        {
        }

        public virtual TResult Click()
        {
            return Click<TResult>();
        }
    }
}
