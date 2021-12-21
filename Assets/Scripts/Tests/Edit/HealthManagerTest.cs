using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class HealthManagerTest
{
    [Test]
    public void getsHealthValue()
    {
        GameObject actor = new GameObject();
        HealthManager hm = actor.AddComponent<HealthManager>();

        Assert.AreEqual(0, hm.getHealth());
    }

    [Test]
    public void increasesHealth()
    {
        GameObject actor = new GameObject();
        HealthManager hm = actor.AddComponent<HealthManager>();

        hm.changeBy(50);

        Assert.AreEqual(50, hm.getHealth());
    }

    [Test]
    public void decreasesHealth()
    {
        GameObject actor = new GameObject();
        HealthManager hm = actor.AddComponent<HealthManager>();

        hm.changeBy(50);
        hm.changeBy(-20);

        Assert.AreEqual(30, hm.getHealth());
    }

    [Test]
    public void getsHealthPercentage()
    {
        GameObject actor = new GameObject();
        HealthManager hm = actor.AddComponent<HealthManager>();

        hm.intialHealth = 200;
        hm.changeBy(50);

        Assert.AreEqual(0.25, hm.getHealthPrcnt());
    }
}
