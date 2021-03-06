﻿using System;
using Unity.Entities;
using UnityEngine;

namespace ODT.Component
{
    public class CannonComponent : MonoBehaviour 
    {
        public Transform[] cannonTransform;

        public GameObject bulletPrefab;

        public float timeBetweenShoot;

        public float lastShootTime; 
    }
}