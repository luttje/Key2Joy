﻿using Key2Joy.LowLevelInput;
using Newtonsoft.Json;
using SimWinInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Key2Joy.Mapping
{
    [Action(
        Description = "Mouse Move Simulation",
        Visibility = ActionVisibility.Never,
        NameFormat = "Move {0} {1},{2} on Mouse"
    )]
    [ExposesScriptingEnumeration(typeof(Mouse.MoveType))]
    [Util.ObjectListViewGroup(
        Name = "Mouse Move Simulation",
        Image = "mouse"
    )]
    internal class MouseMoveAction : BaseAction
    {        
        [JsonProperty]
        public Mouse.MoveType MoveType { get; set; }

        [JsonProperty]
        public int X { get; set; }
        
        [JsonProperty]
        public int Y { get; set; }

        public MouseMoveAction(string name, string description)
            : base(name, description)
        {
        }

        /// <markdown-doc>
        /// <parent-name>Input</parent-name>
        /// <path>Api/Input</path>
        /// </markdown-doc>
        /// <summary>
        /// Simulate moving the mouse
        /// </summary>
        /// <markdown-example>
        /// Nudges the cursor 100 pixels to the left from where it is now.
        /// <code language="js">
        /// <![CDATA[
        /// Mouse.SimulateMove(-100,0)
        /// ]]>
        /// </code>
        /// </markdown-example>
        /// <markdown-example>
        /// Moves the cursor to an absolute position on the screen.
        /// <code language="lua">
        /// <![CDATA[
        /// Mouse.SimulateMove(1024,1050,MoveType.Absolute)
        /// ]]>
        /// </code>
        /// </markdown-example>
        /// <param name="x">X coordinate to move by/to</param>
        /// <param name="y">Y coordinate to move by/to</param>
        /// <param name="moveType">Whether to move relative to the current cursor position (default) or to an absolute position on screen</param>
        /// <name>Mouse.SimulateMove</name>
        [ExposesScriptingMethod("Mouse.SimulateMove")]
        public async void ExecuteForScript(int x, int y, Mouse.MoveType moveType = Mouse.MoveType.Relative)
        {
            X = x;
            Y = y;
            MoveType = moveType;
            
            await Execute();
        }

        internal override async Task Execute(InputBag inputBag = null)
        {
            SimulatedMouse.Move(X, Y, MoveType);
        }

        public override string GetNameDisplay()
        {
            return Name.Replace("{0}", MoveType == Mouse.MoveType.Absolute ? "To" : "By")
                .Replace("{1}", X.ToString())
                .Replace("{2}", Y.ToString());
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MouseMoveAction action))
                return false;

            return action.MoveType == MoveType
                && action.X == X
                && action.Y == Y;
        }

        public override object Clone()
        {
            return new MouseMoveAction(Name, description)
            {
                MoveType = MoveType,
                X = X,
                Y = Y,
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}