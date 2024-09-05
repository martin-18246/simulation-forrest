using UnityEngine;

public class AnimatorScript : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Animal animalBehavior;
    public bool isFox;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (isFox) // this is needed because the animations were in different part of the object tree
        {
            animalBehavior = GetComponentInParent<Animal>();
        } else
        {
            animalBehavior = GetComponent<Animal>();
        }
        
    }

    void Update()
    {
        animator.SetBool("IsWalking", animalBehavior.GetAnimationStateAnimal() == AnimationStateAnimal.Jumping);
    }


}


public enum AnimationStateAnimal // in the end, I did not use but two of these
{
    Idling,
    Walking,
    Jumping,
    Scratching,
    LookingAround
}
