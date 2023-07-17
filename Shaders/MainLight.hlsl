void MainLight_float(out float3 lightDir) 
{
	// setea la preview de los nodos del shader graph

	#ifdef SHADERGRAPH_PREVIEW

	lightDir = float3(0.5, 0.5, 0);
		#else 
		Light Mainlight = GetMainLight();

			//direction para el rimlight
			lightDir = Mainlight.direction;
	#endif
}