
import 'package:flutter/material.dart';
import 'package:dio/dio.dart';
import 'package:flutter_unity_widget/flutter_unity_widget.dart';

void main() {
  runApp(MaterialApp(
    home: UnityScreen()
  ));
}

class UnityScreen extends StatefulWidget {
  UnityScreen({
    Key key
  }): super(key: key);

  @override
  _UnityScreenState createState() => _UnityScreenState();
}

class _UnityScreenState extends State < UnityScreen > {
    String background = "";
    bool categories = true;
    List<Map> myProducts = [];
    bool overlay = true;
    List<Map> slugs = [];

  static final GlobalKey < ScaffoldState > _scaffoldKey =
  GlobalKey < ScaffoldState > ();

  UnityWidgetController _unityWidgetController;

  Widget build(BuildContext context) {

    return Scaffold(
      appBar: overlay ? PreferredSize(
          preferredSize: Size.fromHeight(100.0), // here the desired height
          child: AppBar(flexibleSpace: Image(
          image: background != "" ? NetworkImage(background): NetworkImage("https://media.giphy.com/media/nE9GiM1Wb9pG8/giphy.gif"),
          fit: BoxFit.cover,
        ),backgroundColor: Colors.white)): null,
      key: _scaffoldKey,
      body: Stack(children: 
      [
        Container(
        color: Colors.black,
        child: UnityWidget(
          onUnityCreated: onUnityCreated,
          onUnityMessage: onUnityMessage,
        )), 
        overlay ?  AnimatedCrossFade(
          duration: const Duration(microseconds: 300),
          crossFadeState: categories ? CrossFadeState.showFirst : CrossFadeState.showSecond,
          firstChild:Container(color:Colors.white,child:SafeArea(child:GridView.builder(
            gridDelegate: SliverGridDelegateWithMaxCrossAxisExtent(
                maxCrossAxisExtent: 200,
                childAspectRatio: 4 / 5,
                crossAxisSpacing: 20,
                mainAxisSpacing: 0),
            itemCount: slugs.length,
            itemBuilder: (BuildContext ctx, index) {
              return Container(
                alignment: Alignment.center,
                child: TextButton(onPressed: () => {
                getItems(slugs[index]["slug"]),
                 setState(() {
                   categories = false;
                   background = slugs[index]["bannerImageURL"];
                  })
                },child: Image.network(slugs[index]["image"])));
            }))),
            secondChild: Container(color:Colors.white,child:SafeArea(child:GridView.builder(
            gridDelegate: SliverGridDelegateWithMaxCrossAxisExtent(
                maxCrossAxisExtent: 200,
                childAspectRatio: 4 / 5,
                crossAxisSpacing: 20,
                mainAxisSpacing: 0),
            itemCount: myProducts.length,
            itemBuilder: (BuildContext ctx, index) {
              return Container(
                alignment: Alignment.center,
                child: TextButton(onPressed: () => {
                  this._unityWidgetController.postJsonMessage("ARPlacementInteractable", "setCard", {"tokenIDOverlay":myProducts[index]["id"],"URI":myProducts[index]["image"],"nameOverlay":myProducts[index]["name"],"animation":myProducts[index]["animation"]}),
                  setState(() { overlay = false; }),
                  print("postJsonMessage")
                },child: Image.network(myProducts[index]["image"])));
            })))) : Container(),
        ])
        );
  }
    var account = "";

     void getCategories() async{
  try {
    var response = await Dio().get('https://api.opensea.io/api/v1/collections?asset_owner=$account&limit=300');

      List<Map> cotegories = [];

      for (var item in response.data) {
        cotegories.add({"image":item["image_url"], "bannerImageURL":item["banner_image_url"], "slug":item["slug"]});
      }
      print(cotegories);
      setState(() {
        slugs = cotegories;
      });
  } catch (e) {
    print(e);
  }
}

    void getItems(String slug) async{
  try {
    var response2 = await Dio().get('https://api.opensea.io/api/v1/assets?owner=$account&collection=$slug&offset=0&limit=10');
      
      List<Map> tokens = [];

      for (var item in response2.data["assets"]) {
        tokens.add({"image":item["image_url"], "animation":item["animation_url"], "id":item["token_id"],"name":item["name"]});
      }
      setState(() {
        myProducts = tokens;
      });
  } catch (e) {
    print(e);
  }
}

  // Callback that connects the created controller to the unity controller
  void onUnityCreated(controller) {
    getCategories();
    this._unityWidgetController = controller;
  }

  void onUnityMessage(message) {
    switch (message.toString()) {
      case "overlay":
        setState(() { overlay = true; categories = true;});
        break;
      default:
    }
  }
}
