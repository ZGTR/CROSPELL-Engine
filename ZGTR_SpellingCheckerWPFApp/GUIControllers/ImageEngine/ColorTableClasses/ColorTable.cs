//using System.Collections.Generic;
//using System.Drawing;

//namespace ZGTR_SpellingCheckerWPFApp.GUIControllers.ColorTableClasses
//{
//    public class ColorTable
//    {
//        private Color[] _table;
//        public Color[] Table
//        {
//            get { return _table; }
//        }

//        public ColorTable(OctreeTree.OctreeTypes.AbstractOctree.Octree octree)
//        {
//            _table = new Color[Octree.NumOfLeavesWanted];
//        }

//        public void AddToTable(int key, Color value)
//        {
//            _table[key] =  value;
//        }

//        public void BuildColorTable()
//        {
//            List<Node> leavesNodes = Octree.GetLeavesList();
//            for (int i = 0; i < leavesNodes.Count; i++)
//            {
//                this._table[i] = leavesNodes[i].Color;
//            }
//        }
//    }
//}
