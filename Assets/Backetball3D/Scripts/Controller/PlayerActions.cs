namespace BasketballRash
{
    using InControl;
    using UnityEngine;

    public class PlayerActions : PlayerActionSet
    {
        public PlayerAction Fire;
        public PlayerAction Jump;
        public PlayerAction Left;
        public PlayerAction Right;
        public PlayerAction Up;
        public PlayerAction Down;
        public PlayerTwoAxisAction Move;
		public PlayerAction Shoot;


        public PlayerActions()
        {
            Fire = CreatePlayerAction("Fire");
            Jump = CreatePlayerAction("Jump");
            Left = CreatePlayerAction("Move Left");
            Right = CreatePlayerAction("Move Right");
            Up = CreatePlayerAction("Move Up");
            Down = CreatePlayerAction("Move Down");
            Move = CreateTwoAxisPlayerAction(Left, Right, Down, Up);
			Shoot = CreatePlayerAction("Shoot");
        }


        public static PlayerActions CreateWithDefaultBindings()
        {
            var playerActions = new PlayerActions();

            // How to set up mutually exclusive keyboard bindings with a modifier key.
            // playerActions.Back.AddDefaultBinding( Key.Shift, Key.Tab );
            // playerActions.Next.AddDefaultBinding( KeyCombo.With( Key.Tab ).AndNot( Key.Shift ) );

            playerActions.Fire.AddDefaultBinding(Key.J);
            playerActions.Fire.AddDefaultBinding(InputControlType.Action1);
            // playerActions.Fire.AddDefaultBinding(Mouse.LeftButton);

            playerActions.Jump.AddDefaultBinding(Key.K);
            playerActions.Jump.AddDefaultBinding(InputControlType.Action2);

			playerActions.Shoot.AddDefaultBinding(Key.L);
			playerActions.Shoot.AddDefaultBinding(InputControlType.Action5);

			playerActions.Up.AddDefaultBinding(Key.W);
            playerActions.Down.AddDefaultBinding(Key.S);
            playerActions.Left.AddDefaultBinding(Key.A);
            playerActions.Right.AddDefaultBinding(Key.D);

            playerActions.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
            playerActions.Right.AddDefaultBinding(InputControlType.LeftStickRight);
            playerActions.Up.AddDefaultBinding(InputControlType.LeftStickUp);
            playerActions.Down.AddDefaultBinding(InputControlType.LeftStickDown);

            playerActions.Left.AddDefaultBinding(InputControlType.DPadLeft);
            playerActions.Right.AddDefaultBinding(InputControlType.DPadRight);
            playerActions.Up.AddDefaultBinding(InputControlType.DPadUp);
            playerActions.Down.AddDefaultBinding(InputControlType.DPadDown);

            // playerActions.Up.AddDefaultBinding(Mouse.PositiveY);
            // playerActions.Down.AddDefaultBinding(Mouse.NegativeY);
            // playerActions.Left.AddDefaultBinding(Mouse.NegativeX);
            // playerActions.Right.AddDefaultBinding(Mouse.PositiveX);

            playerActions.ListenOptions.IncludeUnknownControllers = true;
            playerActions.ListenOptions.MaxAllowedBindings = 4;
            //playerActions.ListenOptions.MaxAllowedBindingsPerType = 1;
            //playerActions.ListenOptions.AllowDuplicateBindingsPerSet = true;
            playerActions.ListenOptions.UnsetDuplicateBindingsOnSet = true;
            //playerActions.ListenOptions.IncludeMouseButtons = true;
            //playerActions.ListenOptions.IncludeModifiersAsFirstClassKeys = true;
            //playerActions.ListenOptions.IncludeMouseButtons = true;
            //playerActions.ListenOptions.IncludeMouseScrollWheel = true;

            playerActions.ListenOptions.OnBindingFound = (action, binding) =>
            {
                if (binding == new KeyBindingSource(Key.Escape))
                {
                    action.StopListeningForBinding();
                    return false;
                }
                return true;
            };

            playerActions.ListenOptions.OnBindingAdded += (action, binding) =>
            {
                Debug.Log("Binding added... " + binding.DeviceName + ": " + binding.Name);
            };

            playerActions.ListenOptions.OnBindingRejected += (action, binding, reason) =>
            {
                Debug.Log("Binding rejected... " + reason);
            };

            return playerActions;
        }
    }
}