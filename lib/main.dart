import 'dart:convert';
import 'dart:typed_data';

import 'package:flutter/material.dart';
import 'package:dio/dio.dart';
import 'package:flutter/rendering.dart';
import 'package:flutter_unity_widget/flutter_unity_widget.dart';

void main() {
  runApp(MaterialApp(home: UnityScreen()));
}

class UnityScreen extends StatefulWidget {
  UnityScreen({Key key}) : super(key: key);

  @override
  _UnityScreenState createState() => _UnityScreenState();
}

class _UnityScreenState extends State<UnityScreen> {
  var account = "";
  String background = "";
  bool categories = true;
  bool login = true;
  List<Map> items = [];
  bool overlay = false;
  List<Map> slugs = [];
  TextEditingController walletController = TextEditingController();

  static final GlobalKey<ScaffoldState> _scaffoldKey =
      GlobalKey<ScaffoldState>();

  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();
  UnityWidgetController _unityWidgetController;

  Widget build(BuildContext context) {
    return Scaffold(
        appBar: overlay || (login && overlay)
            ? 
                AppBar(
                  flexibleSpace: Image(
                    height: 110,
                    image: background != "" && background != null
                        ? NetworkImage(background)
                        : AssetImage("assets/images/logo.jpg"),
                    fit: BoxFit.cover,
                  ),
                  backgroundColor: Colors.white,
                  leading: categories == false
                      ? TextButton(
                          child: Text(
                            'Back',
                            style: TextStyle(color: Colors.white),
                          ),
                          onPressed: () {
                            setState(() {
                              categories = true;
                              background = "";
                              items = [];
                            });
                          },
                        )
                      : null,
                )
            : null,
        key: _scaffoldKey,
        body: login && overlay
            ? Form(
                key: _formKey,
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: <Widget>[
                    TextFormField(
                      controller: walletController,
                      decoration: const InputDecoration(
                        hintText: 'Enter your wallet address',
                      ),
                      validator: (String value) {
                        if (value == null ||
                            value.isEmpty ||
                            value.length != 42) {
                          return 'The wallet address is incorrect';
                        }
                        return null;
                      },
                    ),
                    Padding(
                      padding: const EdgeInsets.symmetric(vertical: 16.0),
                      child: ElevatedButton(
                        onPressed: () {
                          // Validate will return true if the form is valid, or false if
                          // the form is invalid.
                          if (_formKey.currentState.validate()) {
                            setState(() {
                              login = false;
                              categories = true;
                              account = walletController.text;
                            });
                          }
                        },
                        child: const Text('Submit'),
                      ),
                    ),
                  ],
                ),
              )
            : Stack(children: [
                Container(
                    color: Colors.black,
                    child: UnityWidget(
                      onUnityCreated: onUnityCreated,
                      onUnityMessage: onUnityMessage,
                    )),
                overlay
                    ? AnimatedCrossFade(
                        duration: const Duration(microseconds: 300),
                        crossFadeState: categories
                            ? CrossFadeState.showFirst
                            : CrossFadeState.showSecond,
                        firstChild: Container(
                            color: Colors.white,
                            height: 1000,
                            child: SafeArea(
                                child: ListView.builder(
                                    scrollDirection: Axis.vertical,
                                    // shrinkWrap: true,
                                    itemCount: slugs.length,
                                    itemBuilder: (BuildContext ctx, index) {
                                      return Container(
                                          alignment: Alignment.center,
                                          child: TextButton(
                                              onPressed: () => {
                                                    print("categories"),
                                                    setState(() {
                                                      categories = false;
                                                      background = slugs[index]
                                                          ["bannerImageURL"];
                                                    }),
                                                    getItems(
                                                        slugs[index]["slug"]),
                                                  },
                                              child: slugs[index]["image"] != ""
                                                  ? Container(
                                                      height: 50,
                                                      child: Flex(
                                                        direction:
                                                            Axis.horizontal,
                                                        mainAxisAlignment:
                                                            MainAxisAlignment
                                                                .start,
                                                        crossAxisAlignment:
                                                            CrossAxisAlignment
                                                                .center,
                                                        children: [
                                                          Spacer(flex: 1),
                                                          Flexible(
                                                              flex: 1,
                                                              child: Image
                                                                  .network(slugs[
                                                                          index]
                                                                      [
                                                                      "image"])),
                                                                      Spacer(flex: 1),
                                                          Flexible(
                                                              flex: 7,
                                                              child: Text(
                                                                slugs[index]
                                                                    ["slug"],
                                                                textAlign:
                                                                    TextAlign
                                                                        .center,
                                                                style: TextStyle(
                                                                  fontSize: 20,
                                                                    color: Colors
                                                                        .black),
                                                              ))
                                                        ],
                                                      ))
                                                  : Text("unsuported")));
                                    }))),
                        secondChild: Container(
                            color: Colors.white,
                            height: 1000,
                            child: GridView.builder(
                                scrollDirection: Axis.vertical,
                                shrinkWrap: true,
                                gridDelegate:
                                    SliverGridDelegateWithMaxCrossAxisExtent(
                                        maxCrossAxisExtent: 200,
                                        childAspectRatio: 4 / 5,
                                        crossAxisSpacing: 20,
                                        mainAxisSpacing: 0),
                                itemCount: items.length,
                                itemBuilder: (BuildContext ctx, index) {
                                  return Container(
                                      alignment: Alignment.center,
                                      child: TextButton(
                                          onPressed: () => {
                                                this
                                                    ._unityWidgetController
                                                    .postJsonMessage(
                                                        "ARPlacementInteractable",
                                                        "setCard", {
                                                  "tokenIDOverlay": items[index]
                                                      ["id"],
                                                  "URI": items[index]["image"],
                                                  "nameOverlay": items[index]
                                                      ["name"],
                                                  "animation": items[index]
                                                      ["animation"] != null ? items[index]
                                                      ["animation"]: "null"
                                                }),
                                                setState(() {
                                                  overlay = false;
                                                }),
                                                print("postJsonMessage")
                                              },
                                          child: Image.network(
                                              items[index]["image"])));
                                })))
                    : Container(),
              ]));
  }

  void getCategories() async {
    try {
      var response = await Dio().get(
          'https://api.opensea.io/api/v1/collections?asset_owner=$account&limit=300');

      List<Map> cotegories = [];

      for (var item in response.data) {
        cotegories.add({
          "image": item["image_url"],
          "bannerImageURL": item["banner_image_url"],
          "slug": item["slug"]
        });
      }
      print(cotegories);
      setState(() {
        slugs = cotegories;
      });
    } catch (e) {
      print(e);
    }
  }

  void getItems(String slug) async {
    try {
      var response2 = await Dio().get(
          'https://api.opensea.io/api/v1/assets?owner=$account&collection=$slug&offset=0&limit=50');

      List<Map> tokens = [];

      for (var item in response2.data["assets"]) {
        tokens.add({
          "image": item["image_url"],
          "animation": item["animation_url"],
          "id": item["token_id"],
          "name": item["name"]
        });
      }
      setState(() {
        items = tokens;
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
        setState(() {
          overlay = true;
          categories = true;
          background = "";
        });
        break;
      default:
    }
  }
}
