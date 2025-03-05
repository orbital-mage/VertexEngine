using System;
using System.Collections.Generic;
using Moq;
using Moq.Sequences;
using VertexEngine.Common.Assets;
using VertexEngine.Common.Assets.Sets;

namespace EngineTests
{
    public static class AssetSetTestData
    {
        public static void SetupIndexedAsset(Mock<IIndexedAsset> mock, int index = 0, string group = "group")
        {
            mock.SetupGet(light => light.Grouping)
                .Returns(group);
            mock.Setup(light => light.GetHashCode())
                .Returns(index);
            mock.Setup(light => light.CompareTo(It.IsAny<IIndexedAsset>()))
                .CallBase();
            
            mock.Setup(light => light.Use(
                    It.IsAny<Dictionary<Type, IAsset>>(),
                    It.IsAny<int>()))
                .InSequence();
        }
    }
}