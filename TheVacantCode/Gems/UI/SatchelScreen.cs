using Godot;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;

namespace TheVacant.TheVacantCode.Gems.UI;

public partial class SatchelScreen : Control, ICapstoneScreen
{
    private const int Columns = 9;
    private const int Rows = 3;
    private static readonly PackedScene Scene = ResourceLoader.Load<PackedScene>("res://TheVacant/scenes/gems/pouch_screen.tscn");
    private static Vector2? _windowPosition;

    private Control _window = null!;
    private readonly List<GemSlot> _slots = [];

    public NetScreenType ScreenType => NetScreenType.CardPile;
    public bool UseSharedBackstop => true;
    public Control? DefaultFocusedControl => _slots.FirstOrDefault();

    public static void Open()
    {
        NDebugAudioManager.Instance?.Play("map_open.mp3");
        NCapstoneContainer.Instance?.Open(new SatchelScreen {Name = nameof(SatchelScreen) });
    }

    public override void _Ready()
    {
        // plain SetAnchorsPreset keeps the 0x0 rect a new node starts with
        SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
        MouseFilter = Control.MouseFilterEnum.Ignore;

        var view = Scene.Instantiate<Control>();
        AddChild(view);
        _window = view.GetNode<Control>("%Window");
        view.GetNode<Label>("%Title").Text = SatchelButton.Title.GetFormattedText();
        view.GetNode<Control>("%Header").GuiInput += Drag;

        var grid = view.GetNode<GridContainer>("%Grid");
        for (var i = 0; i < Columns * Rows; i++)
        {
            var slot = new GemSlot();
            grid.AddChild(slot);
            _slots.Add(slot);
        }
        LinkFocus();
        Redraw();

        if (_windowPosition is { } position)
            Callable.From(() => _window.Position = position).CallDeferred();

        var back = ResourceLoader.Load<PackedScene>("res://scenes/ui/back_button.tscn").Instantiate<NBackButton>();
        AddChild(back);
        back.Released += _ => NCapstoneContainer.Instance?.Close();
        back.Enable();
    }

    public void AfterCapstoneOpened() => Visible = true;

    public void AfterCapstoneClosed()
    {
        Visible = false;
        this.QueueFreeSafely();
    }

    private void Drag(InputEvent input)
    {
        if (input is not InputEventMouseMotion { ButtonMask: MouseButtonMask.Left } motion)
            return;
        _window.Position = (_window.Position + motion.Relative).Clamp(Vector2.Zero, Size - _window.Size);
        _windowPosition = _window.Position;
    }

    private void Redraw()
    {
        var stacks = Pouch.Stacks().ToList();
        for (var i = 0; i < _slots.Count; i++)
            _slots[i].Stack = i < stacks.Count ? stacks[i] : null;
    }

    private void LinkFocus()
    {
        for (var i = 0; i < _slots.Count; i++)
        {
            var (row, col) = (i / Columns, i % Columns);
            _slots[i].FocusNeighborLeft = SlotPath(row, (col + Columns - 1) % Columns);
            _slots[i].FocusNeighborRight = SlotPath(row, (col + 1) % Columns);
            _slots[i].FocusNeighborTop = SlotPath(Math.Max(row - 1, 0), col);
            _slots[i].FocusNeighborBottom = SlotPath(Math.Min(row + 1, Rows - 1), col);
        }
    }

    private NodePath SlotPath(int row, int col) => _slots[row * Columns + col].GetPath();
}
