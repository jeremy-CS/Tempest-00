using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Fluid_Simulation : MonoBehaviour
{
    //Force Variables
    [Header("Forces")]
    public float gravity;
    [Range(0, 1)] 
    public float collisionDamping;

    //Simulation Variables
    [Header("Simulation Variables")]
    public Vector2 boundsSize;
    public bool isSingleActive;
    public bool isMultipleActive;
    [Range(1, 50)]
    public int numOfParticles;
    public float particleSpacing;

    //Particle Variables
    [Header("Particle Variables")]
    public float particleSize;
    public Color particleColor;
    public GameObject particlePrefab;
    public Vector3 startPosition;

    //Single Particle System Variables
    Vector2 position;
    Vector2 velocity;
    GameObject particleEntity;

    //Multiple Particles System Variables
    Vector2[] positions;
    Vector2[] velocities;
    GameObject[] particleEntities;

    // Start is called before the first frame update
    void Start()
    {
        // Single Particle System
        DrawCircle(startPosition, particleSize, Color.red);

        // Multiple Particles System
        CreateParticleArrays();
        DrawCircles();
    }

    // Update is called once per frame
    void Update()
    {
        // -- 1 Particle System -- //
        if (isSingleActive)
        {
            //Apply Forces to particle and handle collisions
            velocity += Vector2.down * gravity * Time.deltaTime;
            position += velocity * Time.deltaTime;
            ResolveCollisions();

            MoveCircle(particleEntity, position);
        }

        // -- Multiple Particles System -- //
        if (isMultipleActive)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                //Apply forces to particles and handle collisions
                velocities[i] += Vector2.down * gravity * Time.deltaTime;
                positions[i] += velocities[i] * Time.deltaTime;
                ResolveCollisions(ref positions[i], ref velocities[i]);

                MoveCircle(particleEntities[i], positions[i]);
            }
        }

        // -- Restart Simulation on Request -- //
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RestartSimulation();
        }
    }

    // Draw circle function
    public void DrawCircle(Vector3 Position, float ParticleSize, Color Color)
    {
        // Maybe create a 2d sprite, not sure right now
        particleEntity = Instantiate(particlePrefab, Position, Quaternion.identity);

        particleEntity.GetComponent<SpriteRenderer>().color = Color;
        particleEntity.transform.localScale *= ParticleSize;
        position = Position;
    }

    // Draw circles function
    public void DrawCircles()
    {
        for (int i = 0; i < numOfParticles; i++)
        {
            particleEntities[i] = Instantiate(particlePrefab, positions[i], Quaternion.identity);

            particleEntities[i].GetComponent<SpriteRenderer>().color = particleColor;
            particleEntities[i].transform.localScale *= particleSize;
        }
    }

    // Creating the necessary arrays for multiple particles system
    public void CreateParticleArrays()
    {
        positions = new Vector2[numOfParticles];
        velocities = new Vector2[numOfParticles];
        particleEntities = new GameObject[numOfParticles];

        // Place particles in a grid formation
        int particlesPerRow = (int)Mathf.Sqrt(numOfParticles);
        int particlesPerColumn = (numOfParticles - 1) / particlesPerRow + 1;
        float spacing = particleSize * 2 + particleSpacing;

        for (int i = 0; i < numOfParticles; i++)
        {
            float x = (i % particlesPerRow - particlesPerRow / 2f + 0.5f) * spacing;
            float y = (i / particlesPerRow - particlesPerColumn / 2f + 0.5f) * spacing;
            positions[i] = new Vector2(x, y);
        }
    }

    // Move circle function
    public void MoveCircle(GameObject particle, Vector2 NewPosition)
    {
        particle.transform.position = NewPosition;
    }

    // Resolve Collision function
    public void ResolveCollisions()
    {
        Vector2 halfBoundSize = boundsSize / 2 - Vector2.one * particleSize;

        if (Mathf.Abs(position.x) > halfBoundSize.x)
        {
            position.x = halfBoundSize.x * Mathf.Sign(position.x);
            velocity.x *= -1 * collisionDamping;
        }

        if (Mathf.Abs(position.y) > halfBoundSize.y)
        {
            position.y = halfBoundSize.y * Mathf.Sign(position.y);
            velocity.y *= -1 * collisionDamping;
        }
    }

    // Resolve Collision function for Multiple Particle System
    public void ResolveCollisions(ref Vector2 Position, ref Vector2 Velocity)
    {
        Vector2 halfBoundSize = boundsSize / 2 - Vector2.one * particleSize;

        if (Mathf.Abs(Position.x) > halfBoundSize.x)
        {
            Position.x = halfBoundSize.x * Mathf.Sign(Position.x);
            Velocity.x *= -1 * collisionDamping;
        }

        if (Mathf.Abs(Position.y) > halfBoundSize.y)
        {
            Position.y = halfBoundSize.y * Mathf.Sign(Position.y);
            Velocity.y *= -1 * collisionDamping;
        }
    }

    public void RestartSimulation()
    {
        if (isSingleActive)
        {
            Destroy(particleEntity);

            DrawCircle(startPosition, particleSize, particleColor);
            velocity = Vector2.zero;
        }

        if (isMultipleActive)
        {
            for (int i = 0; i < numOfParticles; i++) 
            {
                Destroy(particleEntities[i]);
                velocities[i] = Vector2.zero;
            }

            CreateParticleArrays();
            DrawCircles();
        }
    }
}
