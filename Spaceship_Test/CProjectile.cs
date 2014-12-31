using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Spaceship_Test
{
    class CProjectile
    {
        #region Members
        private PointF m_Positon = PointF.Empty;
        private SizeF m_Size = SizeF.Empty;
        private double m_dVX = 0.0;
        private double m_dVY = 0.0;
        private bool m_bOutOfField = false;
        private bool m_bExpired = false;
        private DateTime m_dtSpawnTime = DateTime.MinValue;
        private int m_iExpireDuration = 3000;
        private int m_iMaxDivision = 0;
        private int m_iSpreadFactor = 4;
        private int m_iDivision = 0;
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
        #endregion

        #region Initialize
        public void Initialize(PointF f_Position, SizeF f_Size, double f_dVX, double f_dVY)
        {
            this.Initialize(f_Position, f_Size, f_dVX, f_dVY, 0, 0);
        }

        public void Initialize(PointF f_Position, SizeF f_Size, double f_dVX, double f_dVY, int f_iMaxDivision, int f_iSpreadFactor)
        {
            m_Positon = f_Position;
            m_Size = f_Size;
            m_dVX = f_dVX;
            m_dVY = f_dVY;
            m_dtSpawnTime = DateTime.Now;
        }
        #endregion

        #region Update
        public void Update(Size f_FieldSize)
        {
            m_Positon.X += (float)m_dVX;
            m_Positon.Y += (float)m_dVY;

            m_bOutOfField = m_Positon.X + m_Size.Width < 0
                || m_Positon.X > f_FieldSize.Width
                || m_Positon.Y + m_Size.Height < 0
                || m_Positon.Y > f_FieldSize.Height;

            m_bExpired = DateTime.Now >= m_dtSpawnTime.AddMilliseconds(m_iExpireDuration);
        }
        #endregion

        #region Draw
        public void Draw(Graphics f_Graphics, PointF f_Offset, SizeF f_TileSize)
        {
            f_Graphics.FillRectangle(Brushes.LightGreen, f_Offset.X + m_Positon.X * f_TileSize.Width, f_Offset.Y + m_Positon.Y * f_TileSize.Height,
                m_Size.Width * f_TileSize.Width, m_Size.Height * f_TileSize.Height);
        }
        #endregion
    }
}
