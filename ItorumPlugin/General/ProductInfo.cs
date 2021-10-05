using System.Linq;

using TFlex.Model;

namespace Itorum
{
    public static class ProductInfo
    {
        public static string[] BaseInfo(this Document doc)
        {
            var docSnapshot = doc.BreadthSearch();
            return new string[]
            {
                "\r\nОшибки открытия файлов: " + (docSnapshot.Count() - docSnapshot.NoBroken().Count()) +
                " (влияют на дальнейший расчет)",
                "\r\n\nДеталей: \t" + docSnapshot.NoBroken().Part().Count(),
                "  уникальных: \t" + docSnapshot.NoBroken().Part().NoLibrary().Unique().Count(),
                "  адаптивных: \t" + docSnapshot.NoBroken().Adaptive().Part().Count(),
                "\r\nСборок: \t" + docSnapshot.NoBroken().Product().Count(),
                "  уникальных: \t" + docSnapshot.NoBroken().Product().NoLibrary().Unique().Count(),
                "  адаптивных: \t" + docSnapshot.NoBroken().Adaptive().Product().Count(),
                "\r\nВсего: \t\t" + docSnapshot.NoBroken().Count(),
                "  уникальных: \t" + docSnapshot.NoBroken().NoLibrary().Unique().Count(),
                "  адаптивных: \t" + docSnapshot.NoBroken().Adaptive().Count()
            };
        }

        public static string[] ExtendedInfo(this Document doc)
        {
            var docSnapshot = doc.BreadthSearch();
            return new string[]{
                "\r\nОшибки открытия файлов: " + (docSnapshot.Count() - docSnapshot.NoBroken().Count()) +
                " (влияют на дальнейший расчет)",

                "\r\n\nБуква \"S\" в обозначении: \t\t" + docSnapshot.NoBroken().WithS().Count(),
                "\"FLX\" в наименовании: \t\t\t" + docSnapshot.NoBroken().NameWithFLX().Count(),
                "\"MCH\" в наименовании: \t\t\t" + docSnapshot.NoBroken().NameWithMCH().Count(),
                "Библиотечных фрагментов: \t\t" + docSnapshot.NoBroken().OnlyLibrary().Count(),
                "Не совпадают обозначение и имя файла: \t" + docSnapshot.NoBroken().FileNameNoNumber().Count()

            };
        }

        public static string[] TestingInfo(this Document doc)
        {
            var docSnapshot = doc.BreadthSearch();

            return new string[]{
                "\r\nКоличество: " + docSnapshot.Count()
            };
        }
    }
}
