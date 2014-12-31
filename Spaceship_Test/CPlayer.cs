using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Spaceship_Test
{
    class CPlayer
    {
        #region Members
        private PointF m_Positon = PointF.Empty;
        private PointF m_AimPosition = PointF.Empty;
        private PointF m_MousePosition = PointF.Empty;
        private SizeF m_Size = SizeF.Empty;
        private double m_dAX = 0.01;
        private double m_dAY = 0.01;
        private double m_dVX = 0.0;
        private double m_dVY = 0.0;
        private double m_dVXMax = 0.5;
        private double m_dVYMax = 0.5;
        private int m_iXDir = 0;
        private int m_iYDir = 0;
        private bool m_bShootingActive = false;
        private List<CProjectile> m_lstProjectiles = null;
        private DateTime m_dtLastShoot = DateTime.MinValue;
        private int m_iShootIntervall = 5;
        private string m_strDebugText = string.Empty;
        private Font m_DebugTextFont = null;
        #endregion

        #region Initialize
        public void Initialize(PointF f_Position, SizeF f_Size)
        {
            m_Positon = f_Position;
            m_Size = f_Size;

            m_lstProjectiles = new List<CProjectile>();
            m_DebugTextFont = new Font("Arial", 8);
        }
        #endregion

        private void UpdateMovement(Size f_FieldSize)
        {
            double dAX = 0.0;
            double dAY = 0.0;
            double dVXDir = 0.0;
            double dVYDir = 0.0;

            if (m_dVX > 0)
            {
                dVXDir = 1.0;
            }
            else if (m_dVX < 0)
            {
                dVXDir = -1.0;
            }

            if (m_dVY > 0)
            {
                dVYDir = 1.0;
            }
            else if (m_dVY < 0)
            {
                dVYDir = -1.0;
            }

            if (m_iXDir != 0)
            {
                dAX = m_dAX * (double)m_iXDir;
            }
            else
            {
                dAX = m_dAX / 2.0 * dVXDir * -1;

                if (Math.Abs(m_dVX) <= Math.Abs(dAX))
                {
                    dAX = m_dVX * -1;
                }
            }

            if (m_iYDir != 0)
            {
                dAY = m_dAY * (double)m_iYDir;
            }
            else
            {
                dAY = m_dAY / 2.0 * dVYDir * -1;

                if (Math.Abs(m_dVY) <= Math.Abs(dAY))
                {
                    dAY = m_dVY * -1;
                }
            }

            m_dVX += dAX;
            m_dVY += dAY;

            if (Math.Abs(m_dVX) > m_dVXMax)
            {
                m_dVX = m_dVXMax * dVXDir;
            }

            if (Math.Abs(m_dVY) > m_dVYMax)
            {
                m_dVY = m_dVYMax * dVYDir;
            }

            m_Positon.X += (float)m_dVX;
            m_Positon.Y += (float)m_dVY;

            if (m_Positon.X < 0)
            {
                m_Positon.X = 0;
                m_dVX = -m_dVX / 2.0;
            }
            else if (m_Positon.X + m_Size.Width > f_FieldSize.Width)
            {
                m_Positon.X = f_FieldSize.Width - m_Size.Width;
                m_dVX = -m_dVX / 2.0;
            }

            if (m_Positon.Y < 0)
            {
                m_Positon.Y = 0;
                m_dVY = -m_dVY / 2.0;
            }
            else if (m_Positon.Y + m_Size.Height > f_FieldSize.Height)
            {
                m_Positon.Y = f_FieldSize.Height - m_Size.Height;
                m_dVY = -m_dVY / 2.0;
            }
        }

        private void UpdateAiming(ref double f_dAimDirection)
        {
            f_dAimDirection = 0.0;
            PointF centerPosition = PointF.Empty;

            centerPosition = GetCenterPosition();

            f_dAimDirection = Math.Atan2(m_MousePosition.Y - centerPosition.Y, m_MousePosition.X - centerPosition.X);

            m_AimPosition.X = centerPosition.X + (float)(Math.Cos(f_dAimDirection) * 2.0);
            m_AimPosition.Y = centerPosition.Y + (float)(Math.Sin(f_dAimDirection) * 2.0);
        }

        private void UpdateProjectiles(Size f_FieldSize, CProjectile f_Projectile)
        {


            for (int i = 0; i < m_lstProjectiles.Count; i++)
            {
                f_Projectile = m_lstProjectiles[i];
                f_Projectile.Update(f_FieldSize);
                if (f_Projectile.OutOfField == true || f_Projectile.Expired == true)
                {
                    m_lstProjectiles.RemoveAt(i);
                    i--;
                }
            }
        }

        private void UpdateShooting(CProjectile f_Projectile, ref double f_dAimDirection)
        {
            double dVXProjectile = 0.0;
            double dVYProjectile = 0.0;
            PointF projectilePosition = PointF.Empty;
            SizeF projectileSize = SizeF.Empty;

            if (m_bShootingActive == true
                 && DateTime.Now > m_dtLastShoot.AddMilliseconds(m_iShootIntervall))
            {
                dVXProjectile = m_dVX + Math.Cos(f_dAimDirection) * m_dVXMax / 2.0;
                dVYProjectile = m_dVY + Math.Sin(f_dAimDirection) * m_dVYMax / 2.0;

                projectileSize.Width = 0.2f;
                projectileSize.Height = 0.2f;

                projectilePosition.X = m_AimPosition.X - projectileSize.Width / 2.0f;
                projectilePosition.Y = m_AimPosition.Y - projectileSize.Height / 2.0f;

                f_Projectile = new CProjectile();
                f_Projectile.Initialize(projectilePosition, projectileSize, dVXProjectile, dVYProjectile, 2, 4);
                m_lstProjectiles.Add(f_Projectile);

                m_dtLastShoot = DateTime.Now;
            }
        }

        #region Update
        public void Update(Size f_FieldSize)
        {
            CProjectile projectile = null;
            double dAimDirection = 0.0;

            UpdateMovement(f_FieldSize);
            UpdateAiming(ref dAimDirection);
            UpdateProjectiles(f_FieldSize, projectile);
            UpdateShooting(projectile, ref dAimDirection);

            #region Debug
            m_strDebugText = string.Empty;
            m_strDebugText += string.Format("X:\t{0:0.000}\n", m_Positon.X);
            m_strDebugText += string.Format("Y:\t{0:0.000}\n", m_Positon.Y);
            m_strDebugText += string.Format("VX:\t{0:0.000}\n", m_dVX);
            m_strDebugText += string.Format("VY:\t{0:0.000}\n", m_dVY);
            #endregion
        }
        #endregion

        #region Draw
        public void Draw(Graphics f_Graphics, PointF f_Offset, SizeF f_TileSize)
        {
            PointF centerPosition = GetCenterPosition();

            f_Graphics.FillRectangle(Brushes.Green, f_Offset.X + m_Positon.X * f_TileSize.Width, f_Offset.Y + m_Positon.Y * f_TileSize.Height,
                m_Size.Width * f_TileSize.Width, m_Size.Height * f_TileSize.Height);

            f_Graphics.DrawLine(Pens.LightGreen, f_Offset.X + centerPosition.X * f_TileSize.Width, f_Offset.Y + centerPosition.Y * f_TileSize.Height,
                f_Offset.X + m_AimPosition.X * f_TileSize.Width, f_Offset.Y + m_AimPosition.Y * f_TileSize.Height);

            #region Projectiles
            foreach (CProjectile projectile in m_lstProjectiles)
            {
                projectile.Draw(f_Graphics, f_Offset, f_TileSize);
            }
            #endregion

            #region Debug
            if (m_strDebugText != string.Empty)
            {
                f_Graphics.DrawString(m_strDebugText, m_DebugTextFont, Brushes.White, f_Offset.X + 5, f_Offset.Y + 5);
            }
            #endregion
        }
        #endregion

        #region KeyAction
        public void KeyAction(EKeyAction f_eKeyAction, int f_iKeyCode)
        {
            switch (f_iKeyCode)
            {
                case 65: //A
                    if (f_eKeyAction == EKeyAction.Down)
                    {
                        m_iXDir = -1;
                    }
                    else if (f_eKeyAction == EKeyAction.Up)
                    {
                        if (m_iXDir == -1)
                        {
                            m_iXDir = 0;
                        }
                    }
                    break;
                case 68: //D
                    if (f_eKeyAction == EKeyAction.Down)
                    {
                        m_iXDir = 1;
                    }
                    else if (f_eKeyAction == EKeyAction.Up)
                    {
                        if (m_iXDir == 1)
                        {
                            m_iXDir = 0;
                        }
                    }
                    break;
                case 83: //S
                    if (f_eKeyAction == EKeyAction.Down)
                    {
                        m_iYDir = 1;
                    }
                    else if (f_eKeyAction == EKeyAction.Up)
                    {
                        if (m_iYDir == 1)
                        {
                            m_iYDir = 0;
                        }
                    }
                    break;
                case 87: //W
                    if (f_eKeyAction == EKeyAction.Down)
                    {
                        m_iYDir = -1;
                    }
                    else if (f_eKeyAction == EKeyAction.Up)
                    {
                        if (m_iYDir == -1)
                        {
                            m_iYDir = 0;
                        }
                    }
                    break;
            }
        }
        #endregion

        #region MouseAction
        public void MouseAction(EMouseAction f_eMouseAction, EMouseButton f_eMouseButton, PointF f_MousePosition)
        {
            m_MousePosition = f_MousePosition;

            if (f_eMouseAction == EMouseAction.Down)
            {
                if (f_eMouseButton == EMouseButton.Left)
                {
                    m_bShootingActive = true;
                }
            }
            else if (f_eMouseAction == EMouseAction.Up)
            {
                if (f_eMouseButton == EMouseButton.Left)
                {
                    m_bShootingActive = false;
                }
            }
        }
        #endregion

        #region GetCenterPosition
        private PointF GetCenterPosition()
        {
            PointF centerPosition = PointF.Empty;

            centerPosition.X = m_Positon.X + m_Size.Width / 2.0f;
            centerPosition.Y = m_Positon.Y + m_Size.Height / 2.0f;

            return centerPosition;
        }
        #endregion
    }
}
