using Godot;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace TheVacant.TheVacantCode.Gems.UI;

public partial class GemSlot : NClickableControl
{
    private static readonly PackedScene Scene = ResourceLoader.Load<PackedScene>("res://TheVacant/scenes/gems/gem_slot.tscn");
    private static readonly Vector2 HoverScale = Vector2.One * 1.15f;

    private readonly TextureRect _icon;
    private readonly Label _count;
    private readonly NSelectionReticle _reticle;
    private Tween? _hoverTween;
    private (Gems Gem, int Count)? _stack;

    public GemSlot()
    {
        var view = Scene.Instantiate<Control>();
        AddChild(view);
        CustomMinimumSize = view.Size;
        PivotOffset = view.Size / 2;
        FocusMode = FocusModeEnum.All;
        _icon = view.GetNode<TextureRect>("Icon");
        _count = view.GetNode<Label>("Count");

        _reticle = ResourceLoader.Load<PackedScene>("res://scenes/ui/selection_reticle.tscn").Instantiate<NSelectionReticle>();
        _reticle.SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect);
        _reticle.MouseFilter = MouseFilterEnum.Ignore;
        AddChild(_reticle);
    }

    public (Gems Gem, int Count)? Stack
    {
        get => _stack;
        set
        {
            _stack = value;
            _icon.Texture = value?.Gem.Icon();
            _count.Text = value is { Count: > 1 } stack ? stack.Count.ToString() : "";
        }
    }

    public override void _Ready() => ConnectSignals();

    protected override void OnFocus()
    {
        _hoverTween?.Kill();
        _hoverTween = CreateTween();
        _hoverTween.TweenProperty(_icon, "scale", HoverScale, 0.05);
        if (Stack is { } stack)
            NHoverTipSet.CreateAndShow(this, stack.Gem.Tip(), HoverTipAlignment.Center)
                ?.SetGlobalPosition(GlobalPosition + Vector2.Down * Size.Y * 1.5f);
        if (NControllerManager.Instance?.IsUsingDirectionalNavigation == true)
            _reticle.OnSelect();
    }

    protected override void OnUnfocus()
    {
        NHoverTipSet.Remove(this);
        _hoverTween?.Kill();
        _hoverTween = CreateTween();
        _hoverTween.TweenProperty(_icon, "scale", Vector2.One, 0.5).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Expo);
        _reticle.OnDeselect();
    }
}
