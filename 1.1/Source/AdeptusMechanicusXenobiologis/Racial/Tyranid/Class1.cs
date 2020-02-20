﻿using System;
using UnityEngine;
using Verse;

namespace AdeptusMechanicus
{
    // Token: 0x02000CA1 RID: 3233
    internal class SectionLayer_Hive : SectionLayer
    {
        // Token: 0x060047B7 RID: 18359 RVA: 0x0021AD9D File Offset: 0x0021919D
        public SectionLayer_Hive(Section section) : base(section)
        {
            this.relevantChangeTypes = (Verse.MapMeshFlag)TyranidHiveUtility.MapMeshFlag.Hive;
        }

        // Token: 0x17000B30 RID: 2864
        // (get) Token: 0x060047B8 RID: 18360 RVA: 0x0021ADBB File Offset: 0x002191BB
        public override bool Visible
        {
            get
            {
                return DebugViewSettings.drawSnow;
            }
        }

        // Token: 0x060047B9 RID: 18361 RVA: 0x0021ADC4 File Offset: 0x002191C4
        private bool Filled(int index)
        {
            Building building = base.Map.edificeGrid[index];
            return building != null && building.def.Fillage == FillCategory.Full;
        }
        /*
        public override void DrawLayer()
        {
            if (!this.Visible)
            {
                return;
            }
            int count = this.subMeshes.Count;
            for (int i = 0; i < count; i++)
            {
                LayerSubMesh layerSubMesh = this.subMeshes[i];
                Vector3 s = new Vector3(.28f, 1f, .28f);
                Matrix4x4 matrix = default(Matrix4x4);
                matrix.SetTRS(vector, Quaternion.AngleAxis(angle, Vector3.up), s);
                if (layerSubMesh.finalized && !layerSubMesh.disabled)
                {
                    Graphics.DrawMesh(layerSubMesh.mesh, Vector3.zero, Quaternion.identity, layerSubMesh.material, 0);
                }
            }
        }
        */
        // Token: 0x060047BA RID: 18362 RVA: 0x0021ADFC File Offset: 0x002191FC
        public override void Regenerate()
        {
            LayerSubMesh subMesh = base.GetSubMesh(AMXBMatBases.Hive);
            if (subMesh.mesh.vertexCount == 0)
            {
                SectionLayerGeometryMaker_Solid.MakeBaseGeometry(this.section, subMesh, AltitudeLayer.Terrain);
            }
            subMesh.Clear(MeshParts.Colors);
            MapComponent_HiveGrid _AvPHiveCreep = base.Map.GetComponent<MapComponent_HiveGrid>();
            //    Log.Message(string.Format(" 6 {0}", _AvPHiveCreep.DepthGridDirect_Unsafe));
            float[] depthGridDirect_Unsafe = _AvPHiveCreep.DepthGridDirect_Unsafe;
            CellRect cellRect = this.section.CellRect;
            int num = base.Map.Size.z - 1;
            int num2 = base.Map.Size.x - 1;
            bool flag = false;
            CellIndices cellIndices = base.Map.cellIndices;
            for (int i = cellRect.minX; i <= cellRect.maxX; i++)
            {
                for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
                {
                    float num3 = depthGridDirect_Unsafe[cellIndices.CellToIndex(i, j)];
                    int num4 = cellIndices.CellToIndex(i, j - 1);
                    float num5 = (j <= 0) ? num3 : depthGridDirect_Unsafe[num4];
                    num4 = cellIndices.CellToIndex(i - 1, j - 1);
                    float num6 = (j <= 0 || i <= 0) ? num3 : depthGridDirect_Unsafe[num4];
                    num4 = cellIndices.CellToIndex(i - 1, j);
                    float num7 = (i <= 0) ? num3 : depthGridDirect_Unsafe[num4];
                    num4 = cellIndices.CellToIndex(i - 1, j + 1);
                    float num8 = (j >= num || i <= 0) ? num3 : depthGridDirect_Unsafe[num4];
                    num4 = cellIndices.CellToIndex(i, j + 1);
                    float num9 = (j >= num) ? num3 : depthGridDirect_Unsafe[num4];
                    num4 = cellIndices.CellToIndex(i + 1, j + 1);
                    float num10 = (j >= num || i >= num2) ? num3 : depthGridDirect_Unsafe[num4];
                    num4 = cellIndices.CellToIndex(i + 1, j);
                    float num11 = (i >= num2) ? num3 : depthGridDirect_Unsafe[num4];
                    num4 = cellIndices.CellToIndex(i + 1, j - 1);
                    float num12 = (j <= 0 || i >= num2) ? num3 : depthGridDirect_Unsafe[num4];
                    this.vertDepth[0] = (num5 + num6 + num7 + num3) / 4f;
                    this.vertDepth[1] = (num7 + num3) / 2f;
                    this.vertDepth[2] = (num7 + num8 + num9 + num3) / 4f;
                    this.vertDepth[3] = (num9 + num3) / 2f;
                    this.vertDepth[4] = (num9 + num10 + num11 + num3) / 4f;
                    this.vertDepth[5] = (num11 + num3) / 2f;
                    this.vertDepth[6] = (num11 + num12 + num5 + num3) / 4f;
                    this.vertDepth[7] = (num5 + num3) / 2f;
                    this.vertDepth[8] = num3;
                    for (int k = 0; k < 9; k++)
                    {
                        if (this.vertDepth[k] > 0.01f)
                        {
                            ;
                            flag = true;
                        }
                        subMesh.colors.Add(SectionLayer_Hive.HiveDepthColor(this.vertDepth[k]));
                    }
                }
            }
            if (flag)
            {
                subMesh.disabled = false;
                subMesh.FinalizeMesh(MeshParts.Colors);
            }
            else
            {
                subMesh.disabled = true;
            }
        }

        // Token: 0x060047BB RID: 18363 RVA: 0x0021B154 File Offset: 0x00219554
        private static Color32 HiveDepthColor(float snowDepth)
        {
            return Color32.Lerp(SectionLayer_Hive.ColorClear, SectionLayer_Hive.ColorHive, snowDepth);
        }

        // Token: 0x04003127 RID: 12583
        private float[] vertDepth = new float[9];

        // Token: 0x04003128 RID: 12584
        private static readonly Color32 ColorClear = new Color32(75, 75, 75, 0);

        // Token: 0x04003129 RID: 12585
        private static readonly Color32 ColorHive = new Color32(40, 25, 90, 100);
    }
}
