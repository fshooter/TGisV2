  var map;
  var cars = new Map();
  var car_img = new google.maps.MarkerImage("car.bmp");
  var car_img_excep = new google.maps.MarkerImage("car_excep.bmp");
  var gPaths = new Map();
  var path_LatLngs;
  var path_markers = [];
  var path_current_draw;
  var mode = 'none';
  var LAT_MIN = 38;
  var LAT_MAX = 41;
  var LNG_MIN = 116;
  var LNG_MAX = 123;
  var LNG_DELTA = 0.00001;
  var CORRECT_X = 0.006264;
  var CORRECT_Y = 0.001127;
  
  function initialize() {
    var myLatlng = new google.maps.LatLng(38.42, 116.08);
    var myOptions = {
      zoom: 10,
      center: myLatlng,
	  streetViewControl : false,
      mapTypeId: google.maps.MapTypeId.ROADMAP
    }
    map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
	google.maps.event.addListener(map, 'click', function(event) {
    on_click(event.latLng);
  });
    google.maps.event.addListener(map, 'center_changed', on_center_changed);
	google.maps.event.addListener(map, 'zoom_changed', on_zoom_changed);
  }
  function set_center(x, y)
  {
	x += CORRECT_X;
	y += CORRECT_Y;
	map.setCenter(new google.maps.LatLng(y, x))
  }
  function add_car(id){
	var pos = new google.maps.LatLng(0, 0)
	var option = {
		position : pos,
	}
	var marker = new google.maps.Marker(option);
	cars.put(id, marker);
 }
 
 function remove_car(id)
 {
	marker = cars.get(id);
	if(marker == null) return;
	cars.remove(id);
	marker.setMap(null);
 }
 
 function update_car(id, x, y, excep, show)
 {
	x += CORRECT_X;
	y += CORRECT_Y;
	marker = cars.get(id);
	if(marker == null) return;
	if(!show)
	{
		marker.setMap(null);
		return;
	}
	if(excep)
		marker.setIcon(car_img_excep);
	else
		marker.setIcon(car_img);
	marker.setPosition(new google.maps.LatLng(y, x));
	marker.setMap(map);
 }
 function add_path(id, name, points)
 {
	path = gPaths.get(id);
	if(path != null)
	{
		path.setMap(map);
		return;
	}
	points_convert = [];
	for(i = 0; i < points.length / 2; i++)
	{
		points_convert = points_convert.concat(new google.maps.LatLng(points[i * 2 + 1] + CORRECT_Y
		, points[i * 2] + CORRECT_X));
	}
	var option = {
		paths : points_convert
	}
	var path = new google.maps.Polygon(option);
	path.setMap(map);
	gPaths.put(id, path);
 }
 function remove_path(id)
 {
	path = gPaths.get(id);
	if(path == null) return;
	gPaths.remove(id);
	path.setMap(null);
 }
 
 function begin_draw_path()
 {
	path_LatLngs = [];
	mode = 'draw_path';
	path_current_draw = null;
 }
 function end_draw_path()
 {
	clearPolygnMarker();
	clearPolygn();
	if(path_LatLngs.length < 3)
	{
		path_LatLngs = [];
		return null;
	}
	result_points = [];
	for(i = 0; i < path_LatLngs.length; ++i)
	{
		result_points = result_points.concat(path_LatLngs[i].lng() - CORRECT_X).concat(path_LatLngs[i].lat() - CORRECT_Y);
	}
	path_LatLngs = [];
	return result_points.toString();
 }
 function on_click(location)
 {
	if(mode == 'draw_path')
	{
		path_LatLngs = path_LatLngs.concat(location);
		if(path_LatLngs.length < 3)
			placeMarker(location);
		else
			polygnMarker(path_LatLngs);
	}
 }
 function placeMarker(location) {
  
  var clickedLocation = new google.maps.LatLng(location);
  var marker = new google.maps.Marker({
      position: location, 
      map: map,
  });
  path_markers = path_markers.concat(marker);
 }
 function clearPolygnMarker()
 {
	for(i = 0; i < path_markers.length; ++i)
		path_markers[i].setMap(null);
	path_markers = [];
 }
 function clearPolygn()
 {
	if(path_current_draw != null)
	{
		path_current_draw.setMap(null);
		path_current_draw = null;
	}
 }
 function polygnMarker(LatLngs)
 {
	clearPolygnMarker();
	clearPolygn()
	var opt = {
		path : LatLngs
	}
	var polygn = new google.maps.Polygon(opt);
	polygn.setMap(map);
	path_current_draw = polygn;
 }
 function on_center_changed()
 {
	latlng = map.getCenter();
	var newlat = latlng.lat();
	var newlng = latlng.lng();
	var changed = false;
	if(latlng.lat() < LAT_MIN)
	{	newlat = LAT_MIN + LNG_DELTA; changed = true; }
	if(latlng.lat() > LAT_MAX)
	{	newlat = LAT_MAX - LNG_DELTA; changed = true; }
	if(latlng.lng() < LNG_MIN)
	{	newlng = LNG_MIN + LNG_DELTA; changed = true; }
	if(latlng.lng() > LNG_MAX)
	{	newlng = LNG_MAX - LNG_DELTA; changed = true; }
	if(changed)
	{
		var latlng2 = new google.maps.LatLng(newlat, newlng, true);
		map.setCenter(latlng2);
	}
 }
 function on_zoom_changed()
 {
	if(map.getZoom() < 10)
		map.setZoom(10);
	if(map.getZoom() > 14)
		map.setZoom(14);
 }