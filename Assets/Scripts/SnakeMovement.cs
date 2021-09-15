using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    public List<BodyPart> bodyParts;
    public float moveInterval = 1f;
    float interlopedMoveInterval = 1;
    public int initalTailLength = 10;
    public GameObject tail;
    List<MovementVector> movements = new List<MovementVector>();

    public Counter initSpawnSize;
    public Counter gridSize;
    public Counter speed;

    public void Begin()
    {
        moveInterval = new float[] { 0.5f, 0.4f, 0.3f, 0.2f, 0.1f }[SaveData.values["speed"] - 1];
        initalTailLength = new int[] { 3, 6, 9, 12, 15 }[SaveData.values["initSpawnSize"] - 1];
        float wallDistance = new float[] { 5, 7.5f, 10, 12.5f, 15 }[SaveData.values["gridSize"] - 1];
        GameObject.Find("BOWall").transform.position = new Vector3(0, -wallDistance, 0);
        GameObject.Find("TWall").transform.position = new Vector3(0, wallDistance, 0);
        GameObject.Find("FWall").transform.position = new Vector3(wallDistance, 0, 0);
        GameObject.Find("BAWall").transform.position = new Vector3(-wallDistance, 0, 0);
        GameObject.Find("LWall").transform.position = new Vector3(0, 0, wallDistance);
        GameObject.Find("RWall").transform.position = new Vector3(0, 0, -wallDistance);

        Color startColor = Color.green;
        float startScale = 0.3f;
        GameObject.Find("head").GetComponent<SnakeHead>().makeApple(1);
        for(int i=0; i<initalTailLength; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject mycube = Instantiate(cube, new Vector3(0, 0, -i), Quaternion.identity);
            Destroy(cube);

            mycube.name = "tail";
            if (i == 0)
            {
                mycube.name = "head";
            }

            Color cubeColor = Color.Lerp(startColor, Color.white, 0.3f);
            float cubeScale = Mathf.Lerp(startScale, 0.9f, 0.3f);
            startColor = cubeColor;
            startScale = cubeScale;

            MeshRenderer cubeRenderer = mycube.GetComponent<MeshRenderer>();
            cubeRenderer.receiveShadows = false;
            cubeRenderer.material.color = cubeColor;
            mycube.transform.localScale = Vector3.one * cubeScale;
            mycube.layer = 2;
            mycube.transform.parent = transform;
            BodyPart cubeClass = mycube.AddComponent<BodyPart>();
            bodyParts.Add(cubeClass);
        }

        GameObject.Find("Camera Pivot").GetComponent<MouseLook>().toPosition = bodyParts[0].gameObject.transform;
        MoveOneStep();
    }

    void Update()
    {
        interlopedMoveInterval = Mathf.Lerp(interlopedMoveInterval, moveInterval, 0.1f);

        if (Input.GetKeyDown(KeyCode.W))
        {
            AddMovementVector(Vector3.forward);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            AddMovementVector(Vector3.back);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            AddMovementVector(Vector3.right);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddMovementVector(Vector3.left);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            AddMovementVector(Vector3.up);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            AddMovementVector(Vector3.down);
        }

        // Delete data
        if(Input.GetKey(KeyCode.Alpha0) && Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public void MoveOneStep()
    {
        MovementVector removeThis = null;
        foreach (BodyPart part in bodyParts)
        {
            if (movements.Count != 0)
            {
                foreach (MovementVector move in movements)
                {
                    if (part.toPosition == move.pos)
                    {
                        part.velocity = move.vel;
                        if (part == bodyParts[bodyParts.Count - 1])
                        {
                            removeThis = move;
                        }
                    }
                }
            }
            part.toPosition += part.velocity;
        }
        if(removeThis != null)
        {
            movements.Remove(removeThis);
        }

        if (!GameObject.Find("head").GetComponent<SnakeHead>().stopped)
            Invoke("MoveOneStep", interlopedMoveInterval);
    }

    void AddMovementVector(Vector3 direction)
    {
        Vector3 prevBlockDir = bodyParts[0].toPosition - bodyParts[1].toPosition;
        Vector3 facingDir = GameObject.Find("Camera Pivot").GetComponent<MouseLook>().dir;
        Vector3 relativeDir = Quaternion.AngleAxis(facingDir.y, Vector3.up) * direction;
        if (prevBlockDir != -relativeDir && GameObject.Find("head").GetComponent<SnakeHead>().started)
        {
            movements.Add(new MovementVector(bodyParts[0].toPosition, relativeDir));
        }
    }

    public void AddSnakeLength()
    {
        Vector3 pos = bodyParts[bodyParts.Count - 1].toPosition - bodyParts[bodyParts.Count - 1].velocity;
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject mycube = Instantiate(cube, pos, Quaternion.identity);
        Destroy(cube);

        mycube.name = "tail";

        mycube.GetComponent<MeshRenderer>().receiveShadows = false;
        mycube.transform.localScale = Vector3.one * 0.9f;
        mycube.layer = 2;
        mycube.transform.parent = transform;
        BodyPart cubeClass = mycube.AddComponent<BodyPart>();
        cubeClass.velocity = bodyParts[bodyParts.Count - 1].velocity;
        bodyParts.Add(cubeClass);
    }

    public GameObject particles;
    public IEnumerator Die()
    {
        GameObject.Find("Explode").GetComponent<AudioSource>().Play();
        foreach (BodyPart part in bodyParts)
        {
            Instantiate(particles, part.transform.position, Quaternion.identity);
            part.GetComponent<MeshRenderer>().enabled = false;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
