#pragma strict
 
var clips : AudioClip[];
var clipIndex : int;
var playAudio : boolean = false;
var audioSource : AudioSource;
 
@script RequireComponent(AudioSource);
 
function Update()
{
    PlaySound();
 
}
 
function PlaySound()
{
    if (audioSource.isPlaying) return;
    yield WaitForSeconds(Random.Range(1, 10));
    if(playAudio && !audioSource.isPlaying)
    {
        clipIndex = Random.Range(0, clips.Length - 1);
        if(!audioSource.isPlaying){
            audioSource.clip = clips[clipIndex];
            audioSource.Play();
        	//audioSource.PlayOneShot(clips[clipIndex], 1.0);
        }
    }
    playAudio = !playAudio;
}