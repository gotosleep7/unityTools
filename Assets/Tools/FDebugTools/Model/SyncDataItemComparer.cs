using System.Collections.Generic;

namespace FDebugTools
{
    public class SyncDataItemComparer
    {
        #region SyncDataItemForMat
        public static bool AreSyncDataEqualForMat(SyncDataForMat item1, SyncDataForMat item2)
        {
            if (item1 == null || item2 == null)
            {
                return false;
            }

            if (item1.targetUser != item2.targetUser)
            {
                return false;
            }

            if (!AreSyncDataItemsEqualForMat(item1.dataList, item2.dataList))
            {
                return false;
            }
            return true;
        }

        private static bool AreSyncDataItemsEqualForMat(List<SyncDataForMatItem> params1, List<SyncDataForMatItem> params2)
        {
            if (params1 == null || params2 == null)
            {
                return false;
            }

            if (params1.Count != params2.Count)
            {
                return false;
            }

            for (int i = 0; i < params1.Count; i++)
            {
                if (!AreSyncDataItemEqualForMat(params1[i], params2[i]))
                {
                    return false;
                }
            }

            return true;
        }



        private static bool AreSyncDataItemEqualForMat(SyncDataForMatItem item1, SyncDataForMatItem item2)
        {
            if (item1 == null || item2 == null)
            {
                return false;
            }

            if (item1.path != item2.path)
            {
                return false;
            }

            if (!AreSyncDataParamsForMatEqual(item1.args, item2.args))
            {
                return false;
            }

            return true;
        }
        private static bool AreSyncDataParamsForMatEqual(List<SyncDataParamForMat> params1, List<SyncDataParamForMat> params2)
        {
            if (params1 == null || params2 == null)
            {
                return false;
            }

            if (params1.Count != params2.Count)
            {
                return false;
            }

            for (int i = 0; i < params1.Count; i++)
            {
                if (!AreSyncDataParamsForMatEqual(params1[i], params2[i]))
                {
                    return false;
                }
            }

            return true;
        }


        private static bool AreSyncDataParamsForMatEqual(SyncDataParamForMat param1, SyncDataParamForMat param2)
        {
            if (param1 == null || param2 == null)
            {
                return false;
            }

            if (param1.GetValue() != param2.GetValue() || param1.GetValue() != param2.GetValue())
            {
                return false;
            }

            return true;
        }

        #endregion

        #region SyncDataItem


        public static bool AreSyncDataItemsEqual(SyncData item1, SyncData item2)
        {
            if (item1 == null || item2 == null)
            {
                return false;
            }

            if (item1.targetUser != item2.targetUser)
            {
                return false;
            }

            if (!AreSyncDataItemsEqual(item1.dataList, item2.dataList))
            {
                return false;
            }

            return true;
        }

        private static bool AreSyncDataItemsEqual(List<SyncDataItem> params1, List<SyncDataItem> params2)
        {
            if (params1 == null || params2 == null)
            {
                return false;
            }

            if (params1.Count != params2.Count)
            {
                return false;
            }

            for (int i = 0; i < params1.Count; i++)
            {
                if (!AreSyncDataItemsEqual(params1[i], params2[i]))
                {
                    return false;
                }
            }

            return true;
        }


        private static bool AreSyncDataItemsEqual(SyncDataItem item1, SyncDataItem item2)
        {
            if (item1 == null || item2 == null)
            {
                return false;
            }

            if (item1.path != item2.path || item1.className != item2.className || item1.methodName != item2.methodName || item1.enable != item2.enable)
            {
                return false;
            }

            if (!AreSyncDataParamsEqual(item1.args, item2.args))
            {
                return false;
            }

            return true;
        }

        private static bool AreSyncDataParamsEqual(List<SyncDataParam> params1, List<SyncDataParam> params2)
        {
            if (params1 == null || params2 == null)
            {
                return false;
            }

            if (params1.Count != params2.Count)
            {
                return false;
            }

            for (int i = 0; i < params1.Count; i++)
            {
                if (!AreSyncDataParamsEqual(params1[i], params2[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool AreSyncDataParamsEqual(SyncDataParam param1, SyncDataParam param2)
        {
            if (param1 == null || param2 == null)
            {
                return false;
            }

            if (param1.GetValue() != param2.GetValue() || param1.GetValue() != param2.GetValue())
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}