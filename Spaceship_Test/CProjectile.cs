using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Spaceship_Test
{
    class CProjectile
    {
        #region Members
        private CVector2D m_Positon = null;
        private CVector2D m_Size = null;
        private CVector2D m_Velocity = null;
        private bool m_bOutOfField = false;
        private bool m_bExpired = false;
        private DateTime m_dtSpawnTime = DateTime.MinValue;
        private int m_iExpireDuration = 3000;
        #endregion

        #region Get/Set
        public bool OutOfField
        {
            get { return m_bOutOfField; }
        }

        public bool Expired
        {
            get { return m_bExpired; }
        }

        public CVector2D Size
        {
            get { return m_Size; }
        }

        public CVector2D Position
        {
            get { return m_Positon; }
        }

        public CVector2D Velocity
        {
            get { return m_Velocity; }
        }
        #endregion

        #region Initialize
        public void Initialize(CVector2D f_Position, CVector2D f_Size, CVector2D f_Velocity)
        {
            m_Positon = f_Position;
            m_Size = f_Size;
            m_Velocity = f_Velocity;
            m_dtSpawnTime = DateTime.Now;
        }
        #endregion

        #region Update
        public void Update(CVector2D f_FieldSize)
        {
            m_Positon += m_Velocity;

            if (m_Positon.X < 0.0)
            {
                m_Positon.X = 0;
                m_Velocity.X = -m_Velocity.X / 2.0;
            }
            else if (m_Positon.X + m_Size.X > f_FieldSize.X)
            {
                m_Positon.X = f_FieldSize.X - m_Size.X;
                m_Velocity.X = -m_Velocity.X / 2.0;
            }

            if (m_Positon.Y < 0.0)
            {
                m_Positon.Y = 0;
                m_Velocity.Y = -m_Velocity.Y / 2.0;
            }
            else if (m_Positon.Y + m_Size.Y > f_FieldSize.Y)
            {
                m_Positon.Y = f_FieldSize.Y - m_Size.Y;
                m_Velocity.Y = -m_Velocity.Y / 2.0;
            }

            /*
            m_bOutOfField = m_Positon.X + m_Size.Width < 0
                || m_Positon.X > f_FieldSize.Width
                || m_Positon.Y + m_Size.Height < 0
                || m_Positon.Y > f_FieldSize.Height;
            */

            m_bExpired = DateTime.Now >= m_dtSpawnTime.AddMilliseconds(m_iExpireDuration);
        }
        #endregion

        #region Draw
        public void Draw(Graphics f_Graphics, PointF f_Offset, SizeF f_TileSize)
        {
            f_Graphics.FillRectangle(Brushes.LightGreen, 
                f_Offset.X + (float)m_Positon.X * f_TileSize.Width,
                f_Offset.Y + (float)m_Positon.Y * f_TileSize.Height,
                (float)m_Size.X * f_TileSize.Width,
                (float)m_Size.Y * f_TileSize.Height);
        }
        #endregion
    }
}
