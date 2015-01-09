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
        private CVector2D m_FieldSize = null;
        private Thread m_tGameloopThread = null;
        private VoidDelegate m_OnDraw = null;
        private bool m_bGameloopActive = false;
        #endregion

        #region Initialize
        public void Initialize(VoidDelegate f_OnDraw)
        {
            m_OnDraw = f_OnDraw;

            m_FieldSize = new CVector2D(40, 30);

            m_Player = new CPlayer();
            m_Player.Initialize(new CVector2D(m_FieldSize.X / 2, m_FieldSize.Y / 2), new CVector2D(2, 1));

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
            double dUpdateFactor = 1.0;

            m_Player.Update(dUpdateFactor, m_FieldSize);
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
            for (float x = 0; x <= m_FieldSize.X; x++)
            {
                f_Graphics.DrawLine(Pens.DarkSlateGray, 
                    offset.X + x * tileSize.Width, 
                    offset.Y, 
                    offset.X + x * tileSize.Width, 
                    offset.Y + (float)m_FieldSize.Y * tileSize.Height);
            }
            for (float y = 0; y <= m_FieldSize.Y; y++)
            {
                f_Graphics.DrawLine(Pens.DarkSlateGray, 
                    offset.X, 
                    offset.Y + y * tileSize.Height,
                    offset.X + (float)m_FieldSize.X * tileSize.Width, 
                    offset.Y + y * tileSize.Height);
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
            CVector2D mousePosition = CVector2D.Empty;

            mousePosition.X = (float)(f_MouseControlPosition.X - offset.X) / tileSize.Width;
            mousePosition.Y = (float)(f_MouseControlPosition.Y - offset.Y) / tileSize.Height;

            m_Player.MouseAction(f_eMouseAction, f_eMouseButton, mousePosition);
        }
        #endregion

        #region GetTileSize
        private SizeF GetTileSize(Size f_ControlSize)
        {
            SizeF tileSize = SizeF.Empty;

            tileSize.Width = (float)(f_ControlSize.Width / m_FieldSize.X);
            tileSize.Height = (float)(f_ControlSize.Height / m_FieldSize.Y);

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

            offset.X = (float)(f_ControlSize.Width - tileSize.Width * m_FieldSize.X) / 2.0f;
            offset.Y = (float)(f_ControlSize.Height - tileSize.Height * m_FieldSize.Y) / 2.0f;

            return offset;
        }
        #endregion
    }
}
