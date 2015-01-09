using System;
using System.Collections.Generic;
using System.Text;

namespace Spaceship_Test
{
    class CVector2D
    {
        #region Members
        private double m_dX;
        private double m_dY;
        #endregion

        #region Get/Set
        public double X
        {
            get { return m_dX; }
            set { m_dX = value; }
        }

        public double Y
        {
            get { return m_dY; }
            set { m_dY = value; }
        }

        public static CVector2D Empty
        {
            get { return new CVector2D(); }
        }
        #endregion

        #region Operator +
        public static CVector2D operator +(CVector2D f_Vector1, CVector2D f_Vector2)
        {
            CVector2D vectorNew = new CVector2D();

            vectorNew.X = f_Vector1.X + f_Vector2.X;
            vectorNew.Y = f_Vector1.Y + f_Vector2.Y;

            return vectorNew;
        }
        #endregion

        #region Operator -
        public static CVector2D operator -(CVector2D f_Vector1, CVector2D f_Vector2)
        {
            CVector2D vectorNew = new CVector2D();

            vectorNew.X = f_Vector1.X - f_Vector2.X;
            vectorNew.Y = f_Vector1.Y - f_Vector2.Y;

            return vectorNew;
        }
        #endregion

        #region Operator *
        public static CVector2D operator *(CVector2D f_Vector1, CVector2D f_Vector2)
        {
            CVector2D vectorNew = new CVector2D();

            vectorNew.X = f_Vector1.X * f_Vector2.X;
            vectorNew.Y = f_Vector1.Y * f_Vector2.Y;

            return vectorNew;
        }

        public static CVector2D operator *(CVector2D f_Vector1, double f_dFactor)
        {
            CVector2D vectorNew = new CVector2D();

            vectorNew.X = f_Vector1.X * f_dFactor;
            vectorNew.Y = f_Vector1.Y * f_dFactor;

            return vectorNew;
        }
        #endregion

        #region Operator /
        public static CVector2D operator /(CVector2D f_Vector1, CVector2D f_Vector2)
        {
            CVector2D vectorNew = new CVector2D();

            vectorNew.X = f_Vector1.X / f_Vector2.X;
            vectorNew.Y = f_Vector1.Y / f_Vector2.Y;

            return vectorNew;
        }

        public static CVector2D operator /(CVector2D f_Vector1, double f_dFactor)
        {
            CVector2D vectorNew = new CVector2D();

            vectorNew.X = f_Vector1.X / f_dFactor;
            vectorNew.Y = f_Vector1.Y / f_dFactor;

            return vectorNew;
        }
        #endregion

        #region Constructor
        public CVector2D() : this(0.0, 0.0) { }

        public CVector2D(double f_dX, double f_dY)
        {
            m_dX = f_dX;
            m_dY = f_dY;
        }
        #endregion
    }
}
