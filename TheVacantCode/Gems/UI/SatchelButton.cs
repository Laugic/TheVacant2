using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Rooms;

namespace TheVacant.TheVacantCode.Gems.UI;

public partial class SatchelButton : NButton
{
    private const string NodeName = "GemPouchButton";
    private static readonly PackedScene Scene = ResourceLoader.Load<PackedScene>("res://TheVacant/scenes/gems/pouch_button.tscn");
    private static readonly Vector2 HoverScale = Vector2.One * 1.25f;

    public static LocString Title => new("static_hover_tips", "GEMPOUCH-POUCH_BUTTON.title");
    private static LocString Description => new("static_hover_tips", "GEMPOUCH-POUCH_BUTTON.description");

    private readonly Control _icon;
    private readonly Label _count;
    private Tween? _iconTween;
    private IReadOnlyList<CardPile> _piles = [];

    public SatchelButton()
    {
        var view = Scene.Instantiate<Control>();
        AddChild(view);
        Size = view.Size;
        PivotOffset = Size / 2;
        Visible = false;
        _icon = view.GetNode<Control>("Icon");
        _count = view.GetNode<Label>("CountContainer/Count");
    }

    public static Control? Attach(NCombatUi ui, Player player, CombatState combat)
    {
        // activate can run twice on one combat ui
        if (ui.GetNodeOrNull<SatchelButton>(NodeName) is { } existing)
        {
            existing.Bind(player);
            return null;
        }
        var button = new SatchelButton { Name = NodeName };
        ui.AddChildSafely(button);
        button.Bind(player);
        return button;
    }

    public override void _Ready() => ConnectSignals();

    public override void _EnterTree()
    {
        base._EnterTree();
        CombatManager.Instance.CombatEnded += OnCombatEnded;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        CombatManager.Instance.CombatEnded -= OnCombatEnded;
        Unbind();
        _iconTween?.Kill();
    }

    private void Bind(Player player)
    {
        Unbind();
        _piles = player.PlayerCombatState!.AllPiles;
        foreach (var pile in _piles)
            pile.ContentsChanged += UpdateVisibility;
        UpdateCount();
        UpdateVisibility();
    }

    private void Unbind()
    {
        foreach (var pile in _piles)
            pile.ContentsChanged -= UpdateVisibility;
        _piles = [];
    }

    private void UpdateVisibility() =>
        Visible = _piles.Any(pile => pile.Cards.Any(card => card.Tags.Contains(Gem.Tag)));

    private void UpdateCount()
    {
        var total = Pouch.Total;
        _count.Text = total.ToString();
        // vanilla MegaLabel shrinks 26 -> 20; a plain Label has to do it by hand
        _count.AddThemeFontSizeOverride("font_size", total < 100 ? 26 : 20);
    }

    private void OnCombatEnded(CombatRoom _)
    {
        Unbind();
        Visible = false;
        if (NCapstoneContainer.Instance?.CurrentCapstoneScreen is SatchelScreen)
            NCapstoneContainer.Instance.Close();
    }

    protected override void OnFocus()
    {
        NHoverTipSet.CreateAndShow(this, new HoverTip(Title, Description))
            ?.SetGlobalPosition(GlobalPosition + new Vector2(14f, -375f));
        AnimateIcon(HoverScale, Colors.White, 0.05);
    }

    protected override void OnUnfocus()
    {
        NHoverTipSet.Remove(this);
        AnimateIcon(Vector2.One, Colors.White, 0.5);
    }

    protected override void OnPress() => AnimateIcon(Vector2.One, Colors.DarkGray, 0.25, Tween.TransitionType.Cubic);

    protected override void OnRelease()
    {
        AnimateIcon(IsFocused ? HoverScale : Vector2.One, Colors.White, 0.5);
        if (!CombatManager.Instance.IsInProgress)
            return;
        if (NTargetManager.Instance.IsInSelection)
            NTargetManager.Instance.CancelTargeting();
        if (NCapstoneContainer.Instance?.CurrentCapstoneScreen is SatchelScreen)
            NCapstoneContainer.Instance.Close();
        else
            SatchelScreen.Open();
    }

    // timings copied from the vanilla pile buttons
    private void AnimateIcon(Vector2 scale, Color tint, double seconds, Tween.TransitionType transition = Tween.TransitionType.Expo)
    {
        _iconTween?.Kill();
        _iconTween = CreateTween().SetParallel().SetEase(Tween.EaseType.Out).SetTrans(transition);
        _iconTween.TweenProperty(_icon, "scale", scale, seconds);
        _iconTween.TweenProperty(_icon, "modulate", tint, seconds);
    }
}
