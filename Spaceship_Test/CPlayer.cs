using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Spaceship_Test
{
    class CPlayer
    {
        #region Members
        private CVector2D m_Positon = null;
        private CVector2D m_Size = null;
        private CVector2D m_AimPosition = null;
        private CVector2D m_MousePosition = null;
        private CVector2D m_Acceleration = null;
        private CVector2D m_Velocity = null;
        private CVector2D m_VelocityMax = null;
        private double m_dAimDirection = 0.0;
        private int m_iXDir = 0;
        private int m_iYDir = 0;
        private bool m_bShootingActive = false;
        private List<CProjectile> m_lstProjectiles = null;
        private DateTime m_dtLastShoot = DateTime.MinValue;
        private int m_iShootIntervall = 30;
        private string m_strDebugText = string.Empty;
        private Font m_DebugTextFont = null;
        private Random m_Random = null;
        #endregion

        #region Initialize
        public void Initialize(CVector2D f_Position, CVector2D f_Size)
        {
            m_Positon = f_Position;
            m_Size = f_Size;

            m_AimPosition = CVector2D.Empty;
            m_MousePosition = CVector2D.Empty;
            m_Acceleration = new CVector2D(0.03, 0.03);
            m_Velocity = new CVector2D(0.0, 0.0);
            m_VelocityMax = new CVector2D(0.3, 0.3);

            m_Random = new Random();
            m_lstProjectiles = new List<CProjectile>();
            m_DebugTextFont = new Font("Arial", 8);
        }
        #endregion

        #region Update
        public void Update(CVector2D f_FieldSize)
        {
            UpdateMovement(f_FieldSize);
            UpdateAiming();
            UpdateProjectiles(f_FieldSize);
            UpdateShooting();

            #region Debug
            m_strDebugText = string.Empty;
            m_strDebugText += string.Format("X:\t{0:0.000}\n", m_Positon.X);
            m_strDebugText += string.Format("Y:\t{0:0.000}\n", m_Positon.Y);
            m_strDebugText += string.Format("VX:\t{0:0.000}\n", m_Velocity.X);
            m_strDebugText += string.Format("VY:\t{0:0.000}\n", m_Velocity.Y);
            m_strDebugText += string.Format("a:\t{0:0.000}\n", m_dAimDirection);
            m_strDebugText += string.Format("Count:\t{0}\n", m_lstProjectiles.Count);
            #endregion
        }

        #region UpdateMovement
        private void UpdateMovement(CVector2D f_FieldSize)
        {
            CVector2D acceleration = CVector2D.Empty;
            double dVXDir = 0.0;
            double dVYDir = 0.0;

            if (m_Velocity.X > 0)
            {
                dVXDir = 1.0;
            }
            else if (m_Velocity.X < 0)
            {
                dVXDir = -1.0;
            }

            if (m_Velocity.Y > 0)
            {
                dVYDir = 1.0;
            }
            else if (m_Velocity.Y < 0)
            {
                dVYDir = -1.0;
            }

            if (m_iXDir != 0)
            {
                acceleration.X = m_Acceleration.X * (double)m_iXDir;
            }
            else
            {
                acceleration.X = m_Acceleration.X / 2.0 * dVXDir * -1;

                if (Math.Abs(m_Velocity.X) <= Math.Abs(acceleration.X))
                {
                    acceleration.X = m_Velocity.X * -1;
                }
            }

            if (m_iYDir != 0)
            {
                acceleration.Y = m_Acceleration.Y * (double)m_iYDir;
            }
            else
            {
                acceleration.Y = m_Acceleration.Y / 2.0 * dVYDir * -1;

                if (Math.Abs(m_Velocity.Y) <= Math.Abs(acceleration.Y))
                {
                    acceleration.Y = m_Velocity.Y * -1;
                }
            }

            m_Velocity += acceleration;

            if (Math.Abs(m_Velocity.X) > m_VelocityMax.X)
            {
                m_Velocity.X = m_VelocityMax.X * dVXDir;
            }

            if (Math.Abs(m_Velocity.Y) > m_VelocityMax.Y)
            {
                m_Velocity.Y = m_VelocityMax.Y * dVYDir;
            }

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
        }
        #endregion

        #region UpdateAiming
        private void UpdateAiming()
        {
            CVector2D centerPosition = CVector2D.Empty;

            centerPosition = GetCenterPosition();

            m_dAimDirection = Math.Atan2(m_MousePosition.Y - centerPosition.Y, m_MousePosition.X - centerPosition.X);

            m_AimPosition.X = centerPosition.X + (float)(Math.Cos(m_dAimDirection) * 2.0);
            m_AimPosition.Y = centerPosition.Y + (float)(Math.Sin(m_dAimDirection) * 2.0);
        }
        #endregion

        #region UpdateProjectiles
        private void UpdateProjectiles(CVector2D f_FieldSize)
        {
            CProjectile projectile = null;
            CProjectile projectileSpread = null;
            CVector2D projectileVelocity = null;
            CVector2D projectilePosition = null;
            CVector2D projectileSize = null;
            int iSpreadCount = 0;
            double dRandomDirection = 0.0;

            for (int i = 0; i < m_lstProjectiles.Count; i++)
            {
                projectile = m_lstProjectiles[i];
                projectile.Update(f_FieldSize);

                if (projectile.Expired == true)
                {
                    m_lstProjectiles.RemoveAt(i);
                    i--;

                    if(projectile.Size.X > 0.1f && projectile.Size.Y > 0.1f)
                    {
                        iSpreadCount = m_Random.Next(5, 10);

                        for(int j=0; j < iSpreadCount; j++)
                        {
                            dRandomDirection = m_Random.NextDouble() * Math.PI * 2.0;

                            projectileVelocity = new CVector2D();
                            projectileVelocity.X = Math.Cos(dRandomDirection) * m_VelocityMax.X / 2.0;
                            projectileVelocity.Y = Math.Sin(dRandomDirection) * m_VelocityMax.Y / 2.0;

                            projectileSize = new CVector2D();
                            projectileSize.X = projectile.Size.X / (double)m_Random.Next(2, 3);
                            projectileSize.Y = projectile.Size.Y / (double)m_Random.Next(2, 3);

                            projectilePosition = new CVector2D();
                            projectilePosition.X = projectile.Position.X + projectile.Size.X / 2.0 - projectileSize.X / 2.0f;
                            projectilePosition.Y = projectile.Position.Y + projectile.Size.Y / 2.0 - projectileSize.Y / 2.0f;

                            projectileSpread = new CProjectile();
                            projectileSpread.Initialize(projectilePosition, projectileSize, projectileVelocity);
                            m_lstProjectiles.Add(projectileSpread);
                        }
                    }
                }
            }
        }
        #endregion

        #region UpdateShooting
        private void UpdateShooting()
        {
            CProjectile projectile = null;
            CVector2D projectilePosition = null;
            CVector2D projectileSize = null;
            CVector2D projectileVelocity = null;

            if (m_bShootingActive == true
                 && DateTime.Now > m_dtLastShoot.AddMilliseconds(m_iShootIntervall))
            {
                projectileVelocity = new CVector2D();
                projectileVelocity.X = m_Velocity.X + Math.Cos(m_dAimDirection) * m_VelocityMax.X / 2.0;
                projectileVelocity.Y = m_Velocity.Y + Math.Sin(m_dAimDirection) * m_VelocityMax.Y / 2.0;

                projectileSize = new CVector2D();
                projectileSize.X = 0.2f;
                projectileSize.Y = 0.2f;

                projectilePosition = new CVector2D();
                projectilePosition.X = m_AimPosition.X - projectileSize.X / 2.0f;
                projectilePosition.Y = m_AimPosition.Y - projectileSize.Y / 2.0f;

                projectile = new CProjectile();
                projectile.Initialize(projectilePosition, projectileSize, projectileVelocity);
                m_lstProjectiles.Add(projectile);

                m_dtLastShoot = DateTime.Now;
            }
        }
        #endregion
        #endregion

        #region Draw
        public void Draw(Graphics f_Graphics, PointF f_Offset, SizeF f_TileSize)
        {
            CVector2D centerPosition = GetCenterPosition();

            f_Graphics.FillRectangle(Brushes.Green,
                f_Offset.X + (float)m_Positon.X * f_TileSize.Width,
                f_Offset.Y + (float)m_Positon.Y * f_TileSize.Height,
                (float)m_Size.X * f_TileSize.Width,
                (float)m_Size.Y * f_TileSize.Height);

            f_Graphics.DrawLine(Pens.LightGreen, 
                f_Offset.X + (float)centerPosition.X * f_TileSize.Width,
                f_Offset.Y + (float)centerPosition.Y * f_TileSize.Height,
                f_Offset.X + (float)m_AimPosition.X * f_TileSize.Width,
                f_Offset.Y + (float)m_AimPosition.Y * f_TileSize.Height);

            #region Projectiles
            for (int i = 0; i < m_lstProjectiles.Count; i++)
            {
                m_lstProjectiles[i].Draw(f_Graphics, f_Offset, f_TileSize);
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
        public void MouseAction(EMouseAction f_eMouseAction, EMouseButton f_eMouseButton, CVector2D f_MousePosition)
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
        private CVector2D GetCenterPosition()
        {
            CVector2D centerPosition = CVector2D.Empty;

            centerPosition.X = m_Positon.X + m_Size.X / 2.0f;
            centerPosition.Y = m_Positon.Y + m_Size.Y / 2.0f;

            return centerPosition;
        }
        #endregion
    }
}
