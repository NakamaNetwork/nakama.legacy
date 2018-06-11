using System;

namespace TreasureGuide.Common.Helpers
{
    public static class CacheBustingService
    {
        private static string _appended;

        public static string Appended
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_appended))
                {
                    _appended = String.Format("r={0}", DateTime.Now.Ticks);
                }
                return _appended;
            }
        }
    }
}
