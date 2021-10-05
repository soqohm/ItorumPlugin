using TFlex.Model.Model3D;

namespace Itorum
{
    public static class Fragment3DExtensions
    {
        public static string GetNumber(this Fragment3D f)
        {
            return f.GetVariableValue("$Обозначение", false).TextValue;
        }

        public static string GetName(this Fragment3D f)
        {
            return f.GetVariableValue("$Наименование", false).TextValue;
        }

        public static bool IsProduct(this Fragment3D f)
        {
            return f.GetFragmentDocument(true, false).GetFragments3D().Count > 0;
        }
    }
}
