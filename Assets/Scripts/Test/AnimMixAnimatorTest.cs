using ARPG.Animation;
using UnityEngine;
using UnityEngine.Playables;

public class AnimMixAnimatorTest : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private PlayableGraph graph;
    private Mixer mixer;
    private AnimUnit animUnit;
    private AnimMixAnimator animMixAnimator;
    public AnimationClip clip;

    private void Start()
    {
        graph = PlayableGraph.Create();
        mixer = new Mixer(graph);

        animUnit = new AnimUnit(graph, clip, 0.2f);
        animMixAnimator = new AnimMixAnimator(graph, animator, 0.2f);

        mixer.AddInput(animUnit);
        mixer.AddInput(animMixAnimator);

        AnimHelper.SetOutput(graph, GetComponent<Animator>(), mixer);
        AnimHelper.Start(graph);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q");
            mixer.TransitionTo(0);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("G");
            mixer.TransitionTo(1);
            animMixAnimator.CrossFade("1", 0.2f);
        }
    }

    private void OnDestroy()
    {
        if (enabled)
            graph.Destroy();
    }
}