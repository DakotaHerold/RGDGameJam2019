﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jam
{
    public class RoadManager : MonoBehaviour
    {

        public float minSpawnDuration;
        public float maxSpawnDuration;

        private float duration;
        private float timer;

        private bool isActive;

        public Tile[] BreakableRoads;
        public Tile[] PowerupRoads;

    

        // Start is called before the first frame update
        void Start()
        {
            duration = Random.Range(minSpawnDuration, maxSpawnDuration); 
        }

        public void Activate()
        {
            isActive = true;

            BreakRoadFirst(); 
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.Instance.CurrentState != GameManager.GAME_STATE.RUNNING)
                return;

            if (!isActive)
                return;

            timer += Time.deltaTime;

            if(timer >= duration)
            {
                duration = Random.Range(minSpawnDuration, maxSpawnDuration);
                timer = 0.0f; 
                BreakRoad(); 
            }
        }

        void BreakRoadFirst()
        {

            // Calculate breakable roads 
            List<Tile> unoccupiedRoads = new List<Tile>();

            for (int iTile = 0; iTile < BreakableRoads.Length; ++iTile)
            {
                int hTile = iTile - 1;
                int jTile = iTile + 1;

                if (hTile < 0)
                    hTile = BreakableRoads.Length - 1;

                if (jTile >= BreakableRoads.Length)
                    jTile = 0;

                if (!BreakableRoads[hTile].IsBroken && !BreakableRoads[iTile].IsBroken && !BreakableRoads[iTile].IsBroken)
                {
                    if (!BreakableRoads[hTile].trigger.containsPlayer && !BreakableRoads[iTile].trigger.containsPlayer && !BreakableRoads[iTile].trigger.containsPlayer)
                    {
                        unoccupiedRoads.Add(BreakableRoads[iTile]);
                    }
                }
            }


            if (unoccupiedRoads.Count > 0)
            {
                // Break road. If already broken or contains player just return. Will try again in a few secnds
                Tile randRoad = unoccupiedRoads[Random.Range(0, unoccupiedRoads.Count)];
                randRoad.BreakRoad();

                //bool spawnedRoad = false;

                //if (!randRoad.IsBroken || randRoad.trigger.containsPlayer)
                //{
                //    randRoad.BreakRoad();
                //    spawnedRoad = true;
                //}

                //if (!spawnedRoad)
                //    return;

                // Order tiles from nearest players to furthest and then spawn at nearest power tile


                Tile randCurve = PowerupRoads[Random.Range(0, PowerupRoads.Length)];
                randCurve.SpawnPowerup();
            }
        }

        void BreakRoad()
        {

            // Calculate breakable roads 
            List<Tile> unoccupiedRoads = new List<Tile>(); 

            for(int iTile = 0; iTile < BreakableRoads.Length; ++iTile)
            {
                int hTile = iTile - 1;
                int jTile = iTile + 1;

                if (hTile < 0)
                    hTile = BreakableRoads.Length - 1;

                if (jTile >= BreakableRoads.Length)
                    jTile = 0; 

                if(!BreakableRoads[hTile].IsBroken && !BreakableRoads[iTile].IsBroken && !BreakableRoads[iTile].IsBroken)
                {
                    if (!BreakableRoads[hTile].trigger.containsPlayer && !BreakableRoads[iTile].trigger.containsPlayer && !BreakableRoads[iTile].trigger.containsPlayer)
                    {
                        unoccupiedRoads.Add(BreakableRoads[iTile]); 
                    }
                }
            }


            if (unoccupiedRoads.Count > 0)
            {
                // Break road. If already broken or contains player just return. Will try again in a few secnds
                Tile randRoad = unoccupiedRoads[Random.Range(0, unoccupiedRoads.Count)];
                randRoad.BreakRoad();

                //bool spawnedRoad = false;

                //if (!randRoad.IsBroken || randRoad.trigger.containsPlayer)
                //{
                //    randRoad.BreakRoad();
                //    spawnedRoad = true;
                //}

                //if (!spawnedRoad)
                //    return;

                // Order tiles from nearest players to furthest and then spawn at nearest power tile

                float nearestDist = float.MaxValue;
                Tile nearestPowerTile = PowerupRoads[0];


                for (int iCar = 0; iCar < GameManager.Instance.cars.Count; ++iCar)
                {
                    for (int iTile = 0; iTile < PowerupRoads.Length; ++iTile)
                    {
                    
                        Vector3 target = GameManager.Instance.cars[iCar].transform.position + (GameManager.Instance.cars[iCar].transform.forward * 2.0f);
                        float dist = Vector3.Distance(PowerupRoads[iTile].transform.position, target);

                        // Check nearest tile to any car 
                        if(dist < nearestDist)
                        {
                            nearestDist = dist;
                            nearestPowerTile = PowerupRoads[iTile]; 
                        }
                    }
                }

                nearestPowerTile.SpawnPowerup();

                //Tile randCurve = PowerupRoads[Random.Range(0, PowerupRoads.Length)];
                //randCurve.SpawnPowerup();
            }
        }

        public void FixRoad(CarController car)
        {
            // Get Nearest Road 
            Tile nearestRoad = null;
            float nearestDist = float.MaxValue; 
            for(int iRoad = 0; iRoad < BreakableRoads.Length; ++iRoad)
            {
                if(BreakableRoads[iRoad].IsBroken)
                {
                    Vector3 target = BreakableRoads[iRoad].GetBarrierLocation();
                    if (target != Vector3.zero)
                    {
                        float dist = Vector3.Distance(car.transform.position, target); 
                        if(dist < nearestDist)
                        {
                            nearestDist = dist;
                            nearestRoad = BreakableRoads[iRoad]; 
                        }
                    }
                }
            }

            if(nearestRoad == null)
            {
                // No barriers up 
                return; 
            }
            else
            {
                nearestRoad.FixRoad();
                car.DecrementPowerupCount();
            }

        }
    }
}
