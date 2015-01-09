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
        private CVector2D m_RotaryPosition = null;
        private CVector2D m_Size = null;
        private CVector2D m_Velocity = null;
        private bool m_bOutOfField = false;
        private bool m_bExpired = false;
        private DateTime m_dtSpawnTime = DateTime.MinValue;
        private int m_iExpireDuration = 3000;
        private double m_dRotaryAngle = 0.0;
        private double m_dRotaryRadius = 1.0;
        private double m_dRotaryVelocity = 0.3;
        private double m_dVelocityMax = 0.1;
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
        public void Initialize(CVector2D f_Position, CVector2D f_Size, double f_dDirection)
        {
            m_Positon = f_Position;
            m_RotaryPosition = f_Position;
            m_Size = f_Size;
            m_dtSpawnTime = DateTime.Now;

            m_Velocity = new CVector2D();
            m_Velocity.X = Math.Cos(f_dDirection) * m_dVelocityMax;
            m_Velocity.Y = Math.Sin(f_dDirection) * m_dVelocityMax;
        }
        #endregion

        #region Update
        public void Update(double f_dUpdateFactor, CVector2D f_FieldSize)
        {
            m_RotaryPosition += m_Velocity * f_dUpdateFactor;

            if (m_RotaryPosition.X < 0.0)
            {
                m_RotaryPosition.X = 0;
                m_Velocity.X = -m_Velocity.X / 2.0;
            }
            else if (m_RotaryPosition.X > f_FieldSize.X)
            {
                m_RotaryPosition.X = f_FieldSize.X;
                m_Velocity.X = -m_Velocity.X / 2.0;
            }

            if (m_RotaryPosition.Y < 0.0)
            {
                m_RotaryPosition.Y = 0;
                m_Velocity.Y = -m_Velocity.Y / 2.0;
            }
            else if (m_RotaryPosition.Y > f_FieldSize.Y)
            {
                m_RotaryPosition.Y = f_FieldSize.Y;
                m_Velocity.Y = -m_Velocity.Y / 2.0;
            }

            m_dRotaryAngle += m_dRotaryVelocity * f_dUpdateFactor;

            m_Positon.X = m_RotaryPosition.X + Math.Cos(m_dRotaryAngle) * m_dRotaryRadius;
            m_Positon.Y = m_RotaryPosition.Y + Math.Sin(m_dRotaryAngle) * m_dRotaryRadius;

            /*
            m_bOutOfField = m_Positon.X + m_Size.Width < 0
                || m_Positon.X > f_FieldSize.Width
                || m_Positon.Y + m_Size.Height < 0
                || m_Positon.Y > f_FieldSize.Height;
            */

            m_bExpired = DateTime.Now >= m_dtSpawnTime.AddMilliseconds(m_iExpireDuration / f_dUpdateFactor);
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

        #endregion
        }
    }
}
