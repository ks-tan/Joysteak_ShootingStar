using UnityEngine;


public class SpecialEffects : MonoBehaviour
{
	public static SpecialEffects instance;

	public GameObject commends;
	public ParticleSystem auroraEffect;
	public ParticleSystem splashEffect;
	public ParticleSystem explosionEffect;

	void Awake() {
		instance = this;
	}

	void Start() {
		if (auroraEffect == null)
			Debug.LogError ("Missing Aurora Effect!");
		if (splashEffect == null)
			Debug.LogError ("Missing Splash Effect");
	}

	public ParticleSystem makeExplosion(Vector3 position, Color color) {
		ParticleSystem effect = Instantiate(instance.explosionEffect) as ParticleSystem;
		effect.transform.position = new Vector3 (position.x, position.y, position.z);
		effect.startColor = color;
		Destroy(effect.gameObject, effect.startLifetime);
		return effect;
	}

	public ParticleSystem makeBigSplash(Vector3 position) {
		ParticleSystem effect = Instantiate(instance.explosionEffect) as ParticleSystem;
		effect.transform.position = new Vector3 (position.x, position.y, position.z);
		effect.startColor = new Color (173/255f, 208/255f, 238/255f);
		effect.startSize = 0.2f;
		effect.maxParticles = 5000;
		SetEmissionRate (effect, 5000);
		effect.gravityModifier = 4;
		Destroy(effect.gameObject, effect.startLifetime);
		return effect;
	}

	public ParticleSystem makeSmallSplash(Vector3 position) {
		ParticleSystem effect = Instantiate(instance.splashEffect) as ParticleSystem;
		effect.transform.position = position;
		Destroy(effect.gameObject, effect.startLifetime);
		return effect;
	}
		
	public ParticleSystem makeAurora(Vector3 position) {
		ParticleSystem effect = Instantiate(instance.auroraEffect) as ParticleSystem;
		effect.transform.position = new Vector3 (position.x, position.y - 2, position.z);
		effect.transform.SetParent (commends.transform);
		Destroy(effect.gameObject, effect.startLifetime);
		return effect;
	}

	void SetEmissionRate(ParticleSystem particleSystem, float emissionRate) {
		var emission = particleSystem.emission;
		var rate = emission.rate;
		rate.constantMax = emissionRate;
		emission.rate = rate;
	}
}