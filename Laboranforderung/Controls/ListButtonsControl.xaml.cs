using Laboranforderung.Commands;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Laboranforderung.Controls
{
    /// <summary>
    /// Interaktionslogik für ListButtonsControl.xaml
    /// </summary>
    public partial class ListButtonsControl : UserControl
    {
        public ListButtonsControl()
        {
            InitializeComponent();
            RefreshToolTips();
            this.ButtonMoveUp.Click += ButtonMoveUp_Click;
            this.ButtonMoveDown.Click += ButtonMoveDown_Click;
            this.ButtonMoveLeft.Click += ButtonMoveLeft_Click;
            this.ButtonMoveRight.Click += ButtonMoveRight_Click;
            this.ButtonRemove.Click += ButtonRemove_Click;
            this.ButtonAdd.Click += ButtonAdd_Click;
            this.ButtonAddKategorie.Click += ButtonAddKategorie_Click;
            this.ButtonEdit.Click += ButtonEdit_Click;
        }

        #region Commands

        public ICommand MoveDownCommand
        {
            get { return (ICommand)GetValue(MoveDownCommandProperty); }
            set { SetValue(MoveDownCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MoveDownCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MoveDownCommandProperty =
            DependencyProperty.Register("MoveDownCommand", typeof(ICommand), typeof(ListButtonsControl), new PropertyMetadata(null, OnMoveDownCommand));

        private static void OnMoveDownCommand(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListButtonsControl ctl = (ListButtonsControl)d;
            ctl.ButtonMoveDown.Command = (ICommand)e.NewValue;
        }

        public ICommand MoveUpCommand
        {
            get { return (ICommand)GetValue(MoveUpCommandProperty); }
            set { SetValue(MoveUpCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MoveUpCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MoveUpCommandProperty =
            DependencyProperty.Register("MoveUpCommand", typeof(ICommand), typeof(ListButtonsControl), new PropertyMetadata(null, OnMoveUpCommand));

        private static void OnMoveUpCommand(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListButtonsControl ctl = (ListButtonsControl)d;
            ctl.ButtonMoveUp.Command = (ICommand)e.NewValue;
        }

        public ICommand MoveLeftCommand
        {
            get { return (ICommand)GetValue(MoveLeftCommandProperty); }
            set { SetValue(MoveLeftCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MoveLeftCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MoveLeftCommandProperty =
            DependencyProperty.Register("MoveLeftCommand", typeof(ICommand), typeof(ListButtonsControl), new PropertyMetadata(null, OnMoveLeftCommand));

        private static void OnMoveLeftCommand(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListButtonsControl ctl = (ListButtonsControl)d;
            ctl.ButtonMoveLeft.Command = (ICommand)e.NewValue;
        }

        public ICommand MoveRightCommand
        {
            get { return (ICommand)GetValue(MoveRightCommandProperty); }
            set { SetValue(MoveRightCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MoveRightCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MoveRightCommandProperty =
            DependencyProperty.Register("MoveRightCommand", typeof(ICommand), typeof(ListButtonsControl), new PropertyMetadata(null, OnMoveRightCommand));

        private static void OnMoveRightCommand(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListButtonsControl ctl = (ListButtonsControl)d;
            ctl.ButtonMoveRight.Command = (ICommand)e.NewValue;
        }

        public ICommand AddCommand
        {
            get { return (ICommand)GetValue(AddCommandProperty); }
            set { SetValue(AddCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AddCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddCommandProperty =
            DependencyProperty.Register("AddCommand", typeof(ICommand), typeof(ListButtonsControl), new PropertyMetadata(null, OnAddCommand));

        private static void OnAddCommand(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListButtonsControl ctl = (ListButtonsControl)d;
            ctl.ButtonAdd.Command = (ICommand)e.NewValue;
        }

        public ICommand AddKategorieCommand
        {
            get { return (ICommand)GetValue(AddKategorieCommandProperty); }
            set { SetValue(AddKategorieCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AddKategorieCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddKategorieCommandProperty =
            DependencyProperty.Register("AddKategorieCommand", typeof(ICommand), typeof(ListButtonsControl), new PropertyMetadata(null, OnAddKategorieCommand));

        private static void OnAddKategorieCommand(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListButtonsControl ctl = (ListButtonsControl)d;
            ctl.ButtonAddKategorie.Command = (ICommand)e.NewValue;
        }

        public ICommand RemoveCommand
        {
            get { return (ICommand)GetValue(RemoveCommandProperty); }
            set { SetValue(RemoveCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RemoveCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RemoveCommandProperty =
            DependencyProperty.Register("RemoveCommand", typeof(ICommand), typeof(ListButtonsControl), new PropertyMetadata(null, OnRemoveCommand));

        private static void OnRemoveCommand(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListButtonsControl ctl = (ListButtonsControl)d;
            ctl.ButtonRemove.Command = (ICommand)e.NewValue;
        }

        public ICommand EditCommand
        {
            get { return (ICommand)GetValue(EditCommandProperty); }
            set { SetValue(EditCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditCommandProperty =
            DependencyProperty.Register("EditCommand", typeof(ICommand), typeof(ListButtonsControl), new PropertyMetadata(null, OnEditCommand));

        private static void OnEditCommand(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListButtonsControl ctl = (ListButtonsControl)d;
            ctl.ButtonEdit.Command = (ICommand)e.NewValue;
        }

        #endregion

        #region Click Handlers
        // Provide CLR accessors for the event
        public event RoutedEventHandler Click_MoveUp
        {
            add { AddHandler(Click_MoveUpEvent, value); }
            remove { RemoveHandler(Click_MoveUpEvent, value); }
        }

        // Using a RoutedEvent
        public static readonly RoutedEvent Click_MoveUpEvent = EventManager.RegisterRoutedEvent(
            "Click_MoveUp", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ListButtonsControl));



        // Provide CLR accessors for the event
        public event RoutedEventHandler Click_MoveDown
        {
            add { AddHandler(Click_MoveDownEvent, value); }
            remove { RemoveHandler(Click_MoveDownEvent, value); }
        }

        // Using a RoutedEvent
        public static readonly RoutedEvent Click_MoveDownEvent = EventManager.RegisterRoutedEvent(
            "Click_MoveDown", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ListButtonsControl));



        // Provide CLR accessors for the event
        public event RoutedEventHandler Click_MoveLeft
        {
            add { AddHandler(Click_MoveLeftEvent, value); }
            remove { RemoveHandler(Click_MoveLeftEvent, value); }
        }

        // Using a RoutedEvent
        public static readonly RoutedEvent Click_MoveLeftEvent = EventManager.RegisterRoutedEvent(
            "Click_MoveLeft", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ListButtonsControl));



        // Provide CLR accessors for the event
        public event RoutedEventHandler Click_MoveRight
        {
            add { AddHandler(Click_MoveRightEvent, value); }
            remove { RemoveHandler(Click_MoveRightEvent, value); }
        }

        // Using a RoutedEvent
        public static readonly RoutedEvent Click_MoveRightEvent = EventManager.RegisterRoutedEvent(
            "Click_MoveRight", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ListButtonsControl));




        // Provide CLR accessors for the event
        public event RoutedEventHandler Click_Remove
        {
            add { AddHandler(Click_RemoveEvent, value); }
            remove { RemoveHandler(Click_RemoveEvent, value); }
        }

        // Using a RoutedEvent
        public static readonly RoutedEvent Click_RemoveEvent = EventManager.RegisterRoutedEvent(
            "Click_Remove", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ListButtonsControl));




        // Provide CLR accessors for the event
        public event RoutedEventHandler Click_Add
        {
            add { AddHandler(Click_AddEvent, value); }
            remove { RemoveHandler(Click_AddEvent, value); }
        }

        // Using a RoutedEvent
        public static readonly RoutedEvent Click_AddEvent = EventManager.RegisterRoutedEvent(
            "Click_Add", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ListButtonsControl));




        // Provide CLR accessors for the event
        public event RoutedEventHandler Click_AddKategorie
        {
            add { AddHandler(Click_AddKategorieEvent, value); }
            remove { RemoveHandler(Click_AddKategorieEvent, value); }
        }

        // Using a RoutedEvent
        public static readonly RoutedEvent Click_AddKategorieEvent = EventManager.RegisterRoutedEvent(
            "Click_AddKategorie", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ListButtonsControl));



        // Provide CLR accessors for the event
        public event RoutedEventHandler Click_Edit
        {
            add { AddHandler(Click_EditEvent, value); }
            remove { RemoveHandler(Click_EditEvent, value); }
        }

        // Using a RoutedEvent
        public static readonly RoutedEvent Click_EditEvent = EventManager.RegisterRoutedEvent(
            "Click_Edit", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ListButtonsControl));



        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(Click_EditEvent));
        }

        private void ButtonAddKategorie_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(Click_AddKategorieEvent));
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(Click_AddEvent));
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(Click_RemoveEvent));
        }

        private void ButtonMoveRight_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(Click_MoveRightEvent));
        }

        private void ButtonMoveLeft_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(Click_MoveLeftEvent));
        }

        private void ButtonMoveDown_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(Click_MoveDownEvent));
        }

        private void ButtonMoveUp_Click(object sender, RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(Click_MoveUpEvent));
        }
        #endregion

        #region Tooltips
        public string ElementName
        {
            get { return (string)GetValue(ElementNameProperty); }
            set { SetValue(ElementNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ElementName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ElementNameProperty =
            DependencyProperty.Register("ElementName", typeof(string), typeof(ListButtonsControl), new PropertyMetadata("Element", OnToolTipPropertyChanged));



        public Geschlechter Geschlecht
        {
            get { return (Geschlechter)GetValue(GeschlechtProperty); }
            set { SetValue(GeschlechtProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Geschlecht.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GeschlechtProperty =
            DependencyProperty.Register("Geschlecht", typeof(Geschlechter), typeof(ListButtonsControl), new PropertyMetadata(Geschlechter.saechlich, OnToolTipPropertyChanged));

        public string AddKategorieText
        {
            get { return (string)GetValue(AddKategorieTextProperty); }
            set { SetValue(AddKategorieTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AddKategorieText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddKategorieTextProperty =
            DependencyProperty.Register("AddKategorieText", typeof(string), typeof(ListButtonsControl), new PropertyMetadata("Neue Kategorie", OnToolTipPropertyChanged));

        public string AddItemText
        {
            get { return (string)GetValue(AddItemTextProperty); }
            set { SetValue(AddItemTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AddItemText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddItemTextProperty =
            DependencyProperty.Register("AddItemText", typeof(string), typeof(ListButtonsControl), new PropertyMetadata("", OnToolTipPropertyChanged));

        private static void OnToolTipPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListButtonsControl ctl = (ListButtonsControl)d;
            ctl.RefreshToolTips();
        }

        private void RefreshToolTips()
        {
            ButtonAddKategorie.ToolTip = this.AddKategorieText;
            ButtonMoveUp.ToolTip = ElementName + " nach oben verschieben";
            ButtonMoveDown.ToolTip = ElementName + " nach unten verschieben";
            ButtonMoveLeft.ToolTip = ElementName + " nach links verschieben";
            ButtonMoveRight.ToolTip = ElementName + " nach rechts verschieben";
            ButtonRemove.ToolTip = ElementName + " löschen";
            ButtonEdit.ToolTip = ElementName + " bearbeiten";
            if (AddItemText == "")
            {
                switch (Geschlecht)
                {
                    case Geschlechter.maennlich:
                        ButtonAdd.ToolTip = "Neuer " + ElementName;
                        break;
                    case Geschlechter.weiblich:
                        ButtonAdd.ToolTip = "Neue " + ElementName;
                        break;
                    default:
                        ButtonAdd.ToolTip = "Neues " + ElementName;
                        break;
                }

            }
            else
            {
                ButtonAdd.ToolTip = AddItemText;
            }
        }
        #endregion

        #region Enabled / Disabled
        public bool EditEnabled
        {
            get { return (bool)GetValue(EditEnabledProperty); }
            set { SetValue(EditEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditEnabledProperty =
            DependencyProperty.Register("EditEnabled", typeof(bool), typeof(ListButtonsControl), new PropertyMetadata(true, OnEditEnabledChanged));

        private static void OnEditEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListButtonsControl ctl = (ListButtonsControl)d;
            ctl.ButtonEdit.IsEnabled = (bool)e.NewValue;
        }


        public bool MoveLeftEnabled
        {
            get { return (bool)GetValue(MoveLeftEnabledProperty); }
            set { SetValue(MoveLeftEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MoveLeftEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MoveLeftEnabledProperty =
            DependencyProperty.Register("MoveLeftEnabled", typeof(bool), typeof(ListButtonsControl), new PropertyMetadata(true, OnMoveLeftEnabledChanged));

        private static void OnMoveLeftEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListButtonsControl ctl = (ListButtonsControl)d;
            ctl.ButtonMoveLeft.IsEnabled = (bool)e.NewValue;
        }


        public bool MoveRightEnabled
        {
            get { return (bool)GetValue(MoveRightEnabledProperty); }
            set { SetValue(MoveRightEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MoveRightEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MoveRightEnabledProperty =
            DependencyProperty.Register("MoveRightEnabled", typeof(bool), typeof(ListButtonsControl), new PropertyMetadata(true, OnMoveRightEnabledChanged));

        private static void OnMoveRightEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListButtonsControl ctl = (ListButtonsControl)d;
            ctl.ButtonMoveRight.IsEnabled = (bool)e.NewValue;
        }


        public bool MoveUpEnabled
        {
            get { return (bool)GetValue(MoveUpEnabledProperty); }
            set { SetValue(MoveUpEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BlockMoveUpEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MoveUpEnabledProperty =
            DependencyProperty.Register("BlockMoveUpEnabled", typeof(bool), typeof(ListButtonsControl), new PropertyMetadata(true, OnMoveUpEnabledChanged));

        private static void OnMoveUpEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListButtonsControl ctl = (ListButtonsControl)d;
            ctl.ButtonMoveUp.IsEnabled = (bool)e.NewValue;
        }


        public bool MoveDownEnabled
        {
            get { return (bool)GetValue(MoveDownEnabledProperty); }
            set { SetValue(MoveDownEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BlockMoveDownEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MoveDownEnabledProperty =
            DependencyProperty.Register("BlockMoveDownEnabled", typeof(bool), typeof(ListButtonsControl), new PropertyMetadata(true, OnMoveDownEnabledChanged));

        private static void OnMoveDownEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListButtonsControl ctl = (ListButtonsControl)d;
            ctl.ButtonMoveDown.IsEnabled = (bool)e.NewValue;
        }


        public bool AddEnabled
        {
            get { return (bool)GetValue(AddEnabledProperty); }
            set { SetValue(AddEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AddEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddEnabledProperty =
            DependencyProperty.Register("AddEnabled", typeof(bool), typeof(ListButtonsControl), new PropertyMetadata(true, OnAddEnabledChanged));

        private static void OnAddEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListButtonsControl ctl = (ListButtonsControl)d;
            ctl.ButtonAdd.IsEnabled = (bool)e.NewValue;
        }


        public bool AddKategorieEnabled
        {
            get { return (bool)GetValue(AddKategorieEnabledProperty); }
            set { SetValue(AddKategorieEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AddKategorieEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddKategorieEnabledProperty =
            DependencyProperty.Register("AddKategorieEnabled", typeof(bool), typeof(ListButtonsControl), new PropertyMetadata(true, OnAddKategorieEnabledChanged));

        private static void OnAddKategorieEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListButtonsControl ctl = (ListButtonsControl)d;
            ctl.ButtonAddKategorie.IsEnabled = (bool)e.NewValue;
        }


        public bool RemoveEnabled
        {
            get { return (bool)GetValue(RemoveEnabledProperty); }
            set { SetValue(RemoveEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RemoveEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RemoveEnabledProperty =
            DependencyProperty.Register("RemoveEnabled", typeof(bool), typeof(ListButtonsControl), new PropertyMetadata(true, OnRemoveEnabledChanged));

        private static void OnRemoveEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListButtonsControl ctl = (ListButtonsControl)d;
            ctl.ButtonRemove.IsEnabled = (bool)e.NewValue;
        }

        #endregion

        #region Brushes
        public Brush EditBrushEnabled
        {
            get { return (Brush)GetValue(EditBrushEnabledProperty); }
            set { SetValue(EditBrushEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditBrushEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditBrushEnabledProperty =
            DependencyProperty.Register("EditBrushEnabled", typeof(Brush), typeof(ListButtonsControl), new PropertyMetadata(OnEditBrushEnabledChanged));

        private static void OnEditBrushEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListButtonsControl ctl = (ListButtonsControl)d;
            if (e.NewValue == null)
                ctl.ButtonEdit.EnabledBrush = Brushes.Transparent;
            else
                ctl.ButtonEdit.EnabledBrush = (Brush)e.NewValue;
        }



        public Brush EditBrushDisabled
        {
            get { return (Brush)GetValue(EditBrushDisabledProperty); }
            set { SetValue(EditBrushDisabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditBrushDisabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditBrushDisabledProperty =
            DependencyProperty.Register("EditBrushDisabled", typeof(Brush), typeof(ListButtonsControl), new PropertyMetadata(OnEditBrushDisabledChanged));

        private static void OnEditBrushDisabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListButtonsControl ctl = (ListButtonsControl)d;
            if (e.NewValue == null)
                ctl.ButtonEdit.DisabledBrush = Brushes.Transparent;
            else
                ctl.ButtonEdit.DisabledBrush = (Brush)e.NewValue;
        }

        #endregion

        #region Visiblity
        private static Visibility GetVisibility(bool value)
        {
            if (value)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public bool EditVisible
        {
            get { return (bool)GetValue(EditVisibleProperty); }
            set { SetValue(EditVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditVisibleProperty =
            DependencyProperty.Register("EditVisible", typeof(bool), typeof(ListButtonsControl), new PropertyMetadata(true, OnEditVisibleChanged));

        private static void OnEditVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListButtonsControl ctl = (ListButtonsControl)d;
            ctl.ButtonEdit.Visibility = GetVisibility((bool)e.NewValue);
        }


        public bool LeftRightVisible
        {
            get { return (bool)GetValue(LeftRightVisibleProperty); }
            set { SetValue(LeftRightVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LeftRightVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LeftRightVisibleProperty =
            DependencyProperty.Register("LeftRightVisible", typeof(bool), typeof(ListButtonsControl), new PropertyMetadata(true, OnLeftRightVisibleChanged));

        private static void OnLeftRightVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListButtonsControl ctl = (ListButtonsControl)d;
            Visibility vis = GetVisibility((bool)e.NewValue);
            ctl.ButtonMoveLeft.Visibility = vis;
            ctl.ButtonMoveRight.Visibility = vis;
        }


        public bool UpDownVisible
        {
            get { return (bool)GetValue(UpDownVisibleProperty); }
            set { SetValue(UpDownVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UpDownVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UpDownVisibleProperty =
            DependencyProperty.Register("UpDownVisible", typeof(bool), typeof(ListButtonsControl), new PropertyMetadata(true, OnUpDownVisibleChanged));

        private static void OnUpDownVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListButtonsControl ctl = (ListButtonsControl)d;
            Visibility vis = GetVisibility((bool)e.NewValue);
            ctl.ButtonMoveUp.Visibility = vis;
            ctl.ButtonMoveDown.Visibility = vis;
        }


        public bool AddRemoveVisible
        {
            get { return (bool)GetValue(AddRemoveVisibleProperty); }
            set { SetValue(AddRemoveVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AddRemoveVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddRemoveVisibleProperty =
            DependencyProperty.Register("AddRemoveVisible", typeof(bool), typeof(ListButtonsControl), new PropertyMetadata(true, OnAddRemoveVisibleChanged));

        private static void OnAddRemoveVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListButtonsControl ctl = (ListButtonsControl)d;
            Visibility vis = GetVisibility((bool)e.NewValue);
            ctl.ButtonAdd.Visibility = vis;
            ctl.ButtonRemove.Visibility = vis;
        }

        public bool AddKategorieVisible
        {
            get { return (bool)GetValue(AddKategorieVisibleProperty); }
            set { SetValue(AddKategorieVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AddKategorieVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddKategorieVisibleProperty =
            DependencyProperty.Register("AddKategorieVisible", typeof(bool), typeof(ListButtonsControl), new PropertyMetadata(false, OnAddKategorieVisibleChanged));

        private static void OnAddKategorieVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListButtonsControl ctl = (ListButtonsControl)d;
            Visibility vis = GetVisibility((bool)e.NewValue);
            ctl.ButtonAddKategorie.Visibility = vis;
        }

        public bool AddVisible
        {
            get { return (bool)GetValue(AddVisibleProperty); }
            set { SetValue(AddVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AddVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddVisibleProperty =
            DependencyProperty.Register("AddVisible", typeof(bool), typeof(ListButtonsControl), new PropertyMetadata(true, OnAddVisibleChanged));

        private static void OnAddVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListButtonsControl ctl = (ListButtonsControl)d;
            Visibility vis = GetVisibility((bool)e.NewValue);
            ctl.ButtonAdd.Visibility = vis;
        }

        public bool RemoveVisible
        {
            get { return (bool)GetValue(RemoveVisibleProperty); }
            set { SetValue(RemoveVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RemoveVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RemoveVisibleProperty =
            DependencyProperty.Register("RemoveVisible", typeof(bool), typeof(ListButtonsControl), new PropertyMetadata(true, OnRemoveVisibleChanged));

        private static void OnRemoveVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListButtonsControl ctl = (ListButtonsControl)d;
            Visibility vis = GetVisibility((bool)e.NewValue);
            ctl.ButtonRemove.Visibility = vis;
        }



        public IListCommands ListCommands
        {
            get { return (IListCommands)GetValue(ListCommandsProperty); }
            set { SetValue(ListCommandsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ListCommands.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ListCommandsProperty =
            DependencyProperty.Register(nameof(ListCommands), typeof(IListCommands), typeof(ListButtonsControl), new PropertyMetadata(null, OnListCommandsChanged));


        private static void OnListCommandsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListButtonsControl ctl = (ListButtonsControl)d;
            if (e.NewValue is IListCommands listCommands)
            {
                ctl.ButtonMoveUp.Command = listCommands.MoveUp;
                ctl.ButtonMoveDown.Command = listCommands.MoveDown;
                ctl.ButtonAdd.Command = listCommands.Add;
                ctl.ButtonEdit.Command = listCommands.Edit;
                ctl.ButtonRemove.Command = listCommands.Delete;
                if(e.NewValue is IExtendedListCommands extendedListCommands)
                {
                    ctl.AddKategorieVisible = true;
                    ctl.ButtonAddKategorie.Command = extendedListCommands.AddKategorie;
                }
                else
                {
                    ctl.AddKategorieVisible = false;
                }
            }
        }

        #endregion
    }
}
