﻿using PicoGK;
using System.Numerics;

namespace Leap71
{
    using ShapeKernel;

    namespace Simulation
    {
        public class VectorFieldUtil : ITraverseVectorField
        {
            protected VectorField m_oInputField;
            protected VectorField m_oOutputField;
            protected BBox3       m_oBBox;

            /// <summary>
            /// Utility: Returns a vector field with dummy displacement values and the structure of the voxel field.
            /// </summary>
            public static VectorField oGetDummyVectorField(Voxels voxDomain)
            {
                VectorFieldUtil oSetter = new(voxDomain);
                oSetter.Run();
                return oSetter.oGetOutputField();
            }

            protected VectorFieldUtil(Voxels voxDomain)
            {
                m_oInputField   = new VectorField(voxDomain, new Vector3());
                m_oOutputField  = new VectorField(voxDomain, new Vector3());
                m_oBBox         = Sh.oGetBoundingBox(voxDomain);
            }

            protected void Run()
            {
                m_oInputField.TraverseActive(this);
            }

            /// <summary>
            /// Sets the constant value in the output field for every active voxel in the input field.
            /// </summary>
            public void InformActiveValue(in Vector3 vecPosition, in Vector3 vecValue)
            {
                m_oOutputField.SetValue(vecPosition, vecGetDummyDisplacement(vecPosition));
            }

            protected Vector3 vecGetDummyDisplacement(Vector3 vecPt)
            {
                float fXRatio = Uf.fLimitValue((vecPt.X - m_oBBox.vecMin.X) / (m_oBBox.vecMax.X - m_oBBox.vecMin.X), 0f, 1f);
                float fYRatio = Uf.fLimitValue((vecPt.Y - m_oBBox.vecMin.Y) / (m_oBBox.vecMax.Y - m_oBBox.vecMin.Y), 0f, 1f);
                float fZRatio = Uf.fLimitValue((vecPt.Z - m_oBBox.vecMin.Z) / (m_oBBox.vecMax.Z - m_oBBox.vecMin.Z), 0f, 1f);

                float dX = Uf.fTransFixed(0f, 20f, fYRatio);
                float dY = Uf.fTransFixed(-30f, 5f, fZRatio);
                float dZ = Uf.fTransFixed(10f, 0f, fXRatio);

                Vector3 vecDisplacement = new Vector3(dX, dY, dZ);
                return vecDisplacement;
            }

            /// <summary>
            /// Returns the output scalar field.
            /// </summary>
            protected VectorField oGetOutputField()
            {
                return m_oOutputField;
            }
        }
    }
}