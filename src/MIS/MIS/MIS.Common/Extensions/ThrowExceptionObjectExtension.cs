namespace MIS.Common.Extensions
{
    using System;

    public static class ThrowExceptionObjectExtension
    {
        public static object ThrowIfNull(this object obj, string name)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(name);
            }

            return obj;
        }
    }
}