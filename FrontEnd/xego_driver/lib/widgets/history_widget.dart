import 'package:flutter/material.dart';
import 'package:xego_driver/settings/kColors.dart';
import 'package:xego_driver/widgets/cancelled_rides.dart';
import 'package:xego_driver/widgets/completed_rides.dart';

class HistoryWidget extends StatefulWidget {
  const HistoryWidget({super.key});

  @override
  State<HistoryWidget> createState() => _HistoryWidgetState();
}

class _HistoryWidgetState extends State<HistoryWidget>
    with SingleTickerProviderStateMixin {
  late TabController _tabController;
  late Color _tabColor;

  Color _getTabColor(int tabIndex) {
    if (tabIndex == 0) {
      return KColors.kBlue;
    }
    if (tabIndex == 1) {
      return KColors.kDanger;
    }
    return KColors.kPrimaryColor;
  }

  _setTabColor() {
    if (mounted) {
      setState(() {
        _tabColor = _getTabColor(_tabController.index);
      });
    }
  }

  @override
  void initState() {
    // TODO: implement initState
    super.initState();

    _tabColor = _getTabColor(0);
    _tabController = TabController(length: 2, vsync: this);
    _tabController.addListener(() {
      _setTabColor();
    });
  }

  @override
  void dispose() {
    // TODO: implement dispose
    super.dispose();
    _tabController.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final deviceWidth = MediaQuery.of(context).size.width;

    return Container(
      padding: const EdgeInsets.fromLTRB(0, 5, 0, 0),
      color: KColors.kBackgroundColor,
      child: Column(
        children: [
          Container(
            height: 30,
            width: double.infinity,
            margin: const EdgeInsets.fromLTRB(10, 0, 10, 0),
            decoration: BoxDecoration(
              color: Colors.grey[300],
              borderRadius: BorderRadius.circular(
                25.0,
              ),
            ),
            child: TabBar(
                controller: _tabController,
                indicatorSize: TabBarIndicatorSize.tab,
                isScrollable: false,
                indicator: BoxDecoration(
                  borderRadius: BorderRadius.circular(
                    25.0,
                  ),
                  color: _tabColor,
                ),
                labelColor: Colors.white,
                unselectedLabelColor: Colors.black,
                tabs: const [
                  Tab(text: 'Completed'),
                  Tab(text: 'Cancelled'),
                ]),
          ),
          const SizedBox(height: 5),
          Expanded(
            child: TabBarView(
              controller: _tabController,
              children: const [
                CompletedRidesWidget(),
                CancelledRideWidget(),
              ],
            ),
          ),
        ],
      ),
    );
  }
}
