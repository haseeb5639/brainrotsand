using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InHouseAdsController : MonoBehaviour
{
	public InHouseManager AppOpen;
	public InHouseManager NativeInHouse;
	public InHouseManager BannerInHouse;
    public InHouseManager LargeBannerInHouse;
    public InHouseManager InHouseAd;
	public GameObject InHouse;
	#region Singleton
	private static InHouseAdsController _Instance;
	public static InHouseAdsController Instance
	{
		get
		{

			if (!_Instance) _Instance = FindObjectOfType<InHouseAdsController>();

			return _Instance;
		}
	}
	#endregion



	void Awake()
	{
		if (!_Instance) _Instance = this;
	}


    IEnumerator Start()
    {

		yield return new WaitForSecondsRealtime(6f);
		InHouse.SetActive(true);
    }
}
