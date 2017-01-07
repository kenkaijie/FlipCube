using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class TestPlayerController
{
	private IPlayer player;
	private PlayerController playerController;

	[SetUp]
	public void SetUp()
	{
        Debug.logger.logEnabled = false;
	}

	// if we have a player that is already in idle, we should automatically set our state to idle
	[Test]
	public void TestCorrectPlayerControllerState()
	{
		player = GetPlayerMock();
		player.GetState().Returns(PlayerState.IDLE);
		playerController = new PlayerController(player);

		Assert.AreEqual(playerController.GetState(), LogicControllerState.IDLE);
	}

	// if we have a player that is already in idle, we should automatically set our state to idle
	[Test]
	public void TestIncorrectPlayerControllerState()
	{
		player = GetPlayerMock();
		player.GetState().Returns(PlayerState.CREATED);
		playerController = new PlayerController(player);

		Assert.AreNotEqual(playerController.GetState(), LogicControllerState.IDLE);
	}

	[Test]
	public void TestInvalidMoveCausesPlayerControllerToGoIntoError()
	{
		player = GetPlayerMock();
		player.GetState().Returns(PlayerState.IDLE);
		playerController = new PlayerController(player);
	}

	[TearDown]
	public void TearDown()
	{

    }

	private IPlayer GetPlayerMock()
	{
		return Substitute.For<IPlayer>();
	}

}
