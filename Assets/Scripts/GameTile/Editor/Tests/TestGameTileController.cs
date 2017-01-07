using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using NSubstitute;

[TestFixture]
public class TestGameTileController
{

    private IGameTile gameTile;

    // not much to test on the game tile, both activate and deactivate tiles simply play animations,
    // tile state is stored at the controller level
    [Test]
    public void TestTile()
    {
        Vector3 position = Vector3.zero;
        gameTile = GetMockGameTile();
        gameTile.GetPosition().Returns<Vector3>(position);

        GameTileController gameTileController = new GameTileController(gameTile);

        Assert.AreEqual(gameTileController.GetPosition(), position);

    }

    private IGameTile GetMockGameTile()
    {
        return Substitute.For<IGameTile>();
    }
	
}
