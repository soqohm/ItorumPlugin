using System;
using System.Collections.Generic;

using TFlex;

namespace Itorum
{
    public class ItorumFactory : PluginFactory
    {
        public override string Name => "Иторум";
        public override Guid ID => new Guid("{4d1a7673-0fb4-4a80-a2ac-224b88d6bc21}");

        public List<Button> Buttons = new List<Button>()
        {
            new Button(
                "Импорт из CATIA",
                "Получить атрибуты деталей и сборок, импортированных из CATIA",
                Ico.main,
                "Иторум",
                "Изменение атрибутов",
                () => Macro.New((doc) => doc.CatiaRename())),

            new Button(
                "Удалить \"S\"",
                "Удалить букву \"S\" у деталей и сборок",
                Ico.main,
                "Иторум",
                "Изменение атрибутов",
                () => Macro.New((doc) => doc.DeleteS())),

            new Button(
                "С \"вложенными элементами\"",
                "Установить с \"вложенными элементами\" для всех фрагментов текущей сборки",
                Ico.main,
                "Иторум",
                "Изменение атрибутов",
                () => Macro.New((doc) => doc.WithElements())),

            new Button(
                "Правка ссылок",
                "Удалить избыточность путей у фрагментов текущей сборки",
                Ico.main,
                "Иторум",
                "Изменение атрибутов",
                () => Macro.New((doc) => doc.LinkCorrection())),

            new Button(
                "Отчет по ДСИ",
                "Сформировать отчет по деталям и сборкам",
                Ico.getCounts,
                "Иторум",
                "Статистика",
                () => Macro.NewwithLog((doc, path) => doc.BaseInfo().WriteLog(path), "! Отчет по ДСИ.log")),
            new Button(
                "Тест",
                "Тестирование макросов",
                Ico.test,
                "Приложения",
                " ",
                () => Macro.NewwithLog((doc, path) => doc.TestingInfo().WriteLog(path), "! тестирование.log"))
        };
        public override Plugin CreateInstance() => new MyPlugin(this, Buttons);
    }
}