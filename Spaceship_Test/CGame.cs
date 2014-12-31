using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Drawing;

namespace Spaceship_Test
{
    #region Delegate
    delegate void VoidDelegate();
    #endregion

    #region Enum
    enum EKeyAction
    {
        Empty,
        Down,
        Up
    }

    enum EMouseAction
    {
        Empty,
        Down,
        Up,
        Move
    }

    enum EMouseButton
    {
        Empty,
        Left,
        Right,
        Middle
    }
    #endregion

    class CGame
    {
        #region Members
        private CPlayer m_Player = null;
        private Thread m_tGameloopThread = null;
        private VoidDelegate m_OnDraw = null;
        private bool m_bGameloopActive = false;
        private Size m_FieldSize = Size.Empty;
        #endregion

        #region Initialize
        public void Initialize(VoidDelegate f_OnDraw)
        {
            m_OnDraw = f_OnDraw;

            m_FieldSize = new Size(40, 30);

            m_Player = new CPlayer();
            m_Player.Initialize(new PointF(m_FieldSize.Width / 2, m_FieldSize.Height / 2), new Size(2, 1));

            if (m_bGameloopActive == false)
            {
                m_bGameloopActive = true;
                m_tGameloopThread = new Thread(Gameloop);
                m_tGameloopThread.Start();
            }
        }
        #endregion

        #region Deinitialize
        public void Deinitialize()
        {
            m_OnDraw = null;

            if (m_bGameloopActive == true)
            {
                m_bGameloopActive = false;
                m_tGameloopThread.Abort();
            }
        }
        #endregion

        #region Gameloop
        private void Gameloop()
        {
            double dIntervall = 1000.0 / 100.0;
            DateTime dtTime = DateTime.MinValue;

            while (m_bGameloopActive == true)
            {
                dtTime = DateTime.Now;

                Update();

                if (m_OnDraw != null)
                {
                    m_OnDraw();
                }

                while (DateTime.Now < dtTime.AddMilliseconds(dIntervall))
                {
                    Thread.Sleep(1);
                }
            }
        }
        #endregion

        #region Update
        private void Update()
        {
            m_Player.Update(m_FieldSize);
        }
        #endregion

        #region Draw
        public void Draw(Graphics f_Graphics, Size f_ControlSize)
        {
            SizeF tileSize = GetTileSize(f_ControlSize);
            PointF offset = GetOffset(f_ControlSize);

            //Background
            f_Graphics.Clear(Color.Black);

            //Grid
            for (int x = 0; x <= m_FieldSize.Width; x++)
            {
                f_Graphics.DrawLine(Pens.DarkSlateGray, offset.X + x * tileSize.Width, offset.Y, offset.X + x * tileSize.Width, offset.Y + m_FieldSize.Height * tileSize.Height);
            }
            for (int y = 0; y <= m_FieldSize.Height; y++)
            {
                f_Graphics.DrawLine(Pens.DarkSlateGray, offset.X, offset.Y + y * tileSize.Height, offset.X + m_FieldSize.Width * tileSize.Width, offset.Y + y * tileSize.Height);
            }

            //Player
            m_Player.Draw(f_Graphics, offset, tileSize);
        }
        #endregion

        #region KeyAction
        public void KeyAction(EKeyAction f_eKeyAction, int f_iKeyCode)
        {
            m_Player.KeyAction(f_eKeyAction, f_iKeyCode);
        }
        #endregion

        #region MouseAction
        public void MouseAction(EMouseAction f_eMouseAction, EMouseButton f_eMouseButton, Point f_MouseControlPosition, Size f_ControlSize)
        {
            SizeF tileSize = GetTileSize(f_ControlSize);
            PointF offset = GetOffset(f_ControlSize);
            PointF mousePosition = GetOffset(f_ControlSize);

            mousePosition.X = (float)(f_MouseControlPosition.X - offset.X) / tileSize.Width;
            mousePosition.Y = (float)(f_MouseControlPosition.Y - offset.Y) / tileSize.Height;

            m_Player.MouseAction(f_eMouseAction, f_eMouseButton, mousePosition);
        }
        #endregion

        #region GetTileSize
        private SizeF GetTileSize(Size f_ControlSize)
        {
            SizeF tileSize = SizeF.Empty;

            tileSize.Width = f_ControlSize.Width / m_FieldSize.Width;
            tileSize.Height = f_ControlSize.Height / m_FieldSize.Height;

            if(tileSize.Width < tileSize.Height)
            {
                tileSize.Height = tileSize.Width;
            }
            else if (tileSize.Width > tileSize.Height)
            {
                tileSize.Width = tileSize.Height;
            }

            return tileSize;
        }
        #endregion

        #region GetOffset
        private PointF GetOffset(Size f_ControlSize)
        {
            SizeF tileSize = GetTileSize(f_ControlSize);
            PointF offset = PointF.Empty;

            offset.X = (f_ControlSize.Width - tileSize.Width * m_FieldSize.Width) / 2.0f;
            offset.Y = (f_ControlSize.Height - tileSize.Height * m_FieldSize.Height) / 2.0f;

            return offset;
        }
        #endregion
    }
}
