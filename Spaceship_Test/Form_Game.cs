using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Spaceship_Test
{
    public partial class Form_Game : Form
    {
        #region Members
        private CGame m_Game = null;
        #endregion

        #region Construktor
        public Form_Game()
        {
            InitializeComponent();

            m_Game = new CGame();
            m_Game.Initialize(Draw);
        }
        #endregion

        #region OnLoad
        private void Form_Game_Load(object sender, EventArgs e)
        {
            
        }
        #endregion

        #region OnClosing
        private void Form_Game_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_Game.Deinitialize();
        }
        #endregion

        #region Draw
        private void Draw()
        {
            if(pbPicture.InvokeRequired == true)
            {
                pbPicture.Invoke(new VoidDelegate(Draw));
            }
            else
            {
                pbPicture.Invalidate();
            }
        }
        #endregion

        #region OnPaint
        private void pbPicture_Paint(object sender, PaintEventArgs e)
        {
            m_Game.Draw(e.Graphics, pbPicture.ClientSize);
        }
        #endregion

        #region OnKeyDown
        private void Form_Game_KeyDown(object sender, KeyEventArgs e)
        {
            m_Game.KeyAction(EKeyAction.Down, e.KeyValue);
        }
        #endregion

        #region OnKeyUp
        private void Form_Game_KeyUp(object sender, KeyEventArgs e)
        {
            m_Game.KeyAction(EKeyAction.Up, e.KeyValue);
        }
        #endregion

        #region OnMouseDown
        private void pbPicture_MouseDown(object sender, MouseEventArgs e)
        {
            EMouseButton eMouseButton = EMouseButton.Empty;

            if (e.Button == MouseButtons.Left) eMouseButton = EMouseButton.Left;
            else if (e.Button == MouseButtons.Right) eMouseButton = EMouseButton.Right;
            else if (e.Button == MouseButtons.Middle) eMouseButton = EMouseButton.Middle;

            m_Game.MouseAction(EMouseAction.Down, eMouseButton, e.Location, pbPicture.ClientSize);
        }
        #endregion

        #region OnMouseUp
        private void pbPicture_MouseUp(object sender, MouseEventArgs e)
        {
            EMouseButton eMouseButton = EMouseButton.Empty;

            if (e.Button == MouseButtons.Left) eMouseButton = EMouseButton.Left;
            else if (e.Button == MouseButtons.Right) eMouseButton = EMouseButton.Right;
            else if (e.Button == MouseButtons.Middle) eMouseButton = EMouseButton.Middle;

            m_Game.MouseAction(EMouseAction.Up, eMouseButton, e.Location, pbPicture.ClientSize);
        }
        #endregion

        #region OnMouseMove
        private void pbPicture_MouseMove(object sender, MouseEventArgs e)
        {
            EMouseButton eMouseButton = EMouseButton.Empty;

            if (e.Button == MouseButtons.Left) eMouseButton = EMouseButton.Left;
            else if (e.Button == MouseButtons.Right) eMouseButton = EMouseButton.Right;
            else if (e.Button == MouseButtons.Middle) eMouseButton = EMouseButton.Middle;

            m_Game.MouseAction(EMouseAction.Move, eMouseButton, e.Location, pbPicture.ClientSize);
        }
        #endregion
    }
}
