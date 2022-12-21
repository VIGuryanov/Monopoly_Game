using Monopoly_class_library.Lands;

namespace Monopoly_class_library
{
    public static class GameEvents
    {
        public static Func<bool, ActionResult> AskToBuy(UserLandCard land, Player player) =>
            agree => agree ? land.Buy(player) : new ActionResult();
    }
}
