using System.Globalization;

namespace Moyen.Infrastructure
{
    public static class Slug
    {
        public static string GenerateSlug(this string phrase){
            IdnMapping idn = new IdnMapping();
            string punyCode = idn.GetAscii(phrase);
            return punyCode;
        }

    }
}