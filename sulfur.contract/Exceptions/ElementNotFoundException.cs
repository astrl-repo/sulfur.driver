using System;

namespace Sulfur.Contract.Exceptions
{
    public class ElementNotFoundException : Exception
    {
        public ElementNotFoundException(string xpath) : base($"Failed to find an element by xpath: {xpath}")
        {
        }
    }
}