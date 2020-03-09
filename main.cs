using System.Collections.Generic;
using System.Linq;
public class Program {
	public static int MaxProfitWithKTransactions(int[] prices, int k) {
		LinkedList<int[]> trends = new LinkedList<int[]>();
		int tempstart = 0;
		int tempend = 0;
		trends.AddFirst(new int[]{0,0,0,0});
		LinkedListNode<int[]> current = trends.Last;
		
		for(int i = 1; i < prices.Length; i++){
			current = trends.Last; 
			if(prices[i] > prices[i-1]) {
				tempend = i;
				current.Value = new int[]{ tempstart, tempend, prices[tempstart], prices[tempend] };
			}else{
				if(i != prices.Length-1){
					trends.AddLast(new int[]{tempstart,tempend,prices[tempstart],prices[tempend]});
				}
				tempstart = i;
				tempend = i;
			}
		} 
		
		//TRENDS is the initial trends case, there are no limits for how many increases there are. 
		//Now we implement a way of merging or removing the transactions with minimal loss. 
		
		int profit = 0; 
		
		if(k >= trends.Count){ //no minimizing necessary
			//calculate profit full
			foreach(int[] data in trends) {
				profit += (data[3] - data[2]); //profit adds finish - initial
			}
		}
		
		LinkedList<int[]> merging = trends;
		
		for(int i = trends.Count; i > k; i-=1){ //removal / merge
			LinkedListNode<int[]> lowestcausenode; //Just a node.
			int lowestcause;
			LinkedListNode<int[]> currentnode = trends.First;
			bool finalremoval = false;
			for(int index = 0; index < trends.Count; trends++){
				int cause = 0;
				bool isremoval = false;
				int[] data = currentnode.Value; 
				if(currentnode.Equals(trends.First) || currentnode.Equals(trends.Last)){
								if(currentnode.Equals(trends.First)){			//..
									lowestcause = data[3]-data[2];         //this is just to set the lowestcause as a base!!!
								}																					//..
					cause = data[3] - data[2];
					isremoval = true;
				}else{ //MERGE the current into the next and find the cause (PROFIT 1 + PROFIT 2) - (2LAST - 1START)
					int[] next = currentnode.Next.Value;
					int profit = (data[3]-data[2])+(next[3]-next[2]);
					int newprofit = (next[3] - data[2]);					
					cause = profit-newprofit;
				}
				if(currentnode.Equals(trends.First)){
					int[] next = currentnode.Next.Value;
					int profit = (data[3]-data[2])+(next[3]-next[2]);
					int newprofit = (next[3] - data[2]);	
					if(profit-newprofit<cause){
						cause = profit-newprofit;
					}
					
				}
				if(cause < lowestcause){
					lowestcause = cause;
					lowestcausenode = currentnode;
					finalremoval = isremoval;
				}
				currentnode = currentnode.Next;
				if(currentnode == null){
					break;
				}
			}
			if(finalremoval){
				merging.Remove(currentnode);
			}else{
				int[] currentdata = currentnode.Value;
				int[] nextdata = currentnode.Next.Value;
				merging.Remove(currentnode.Next);
				currentnode.Value = new int[] { currentdata[0], nextdata[1], currentdata[2], nextdata[3] };
			}
			
			//merging / removal complete
			
			foreach(int[] data in merging) {
				profit += (data[3] - data[2]); //profit adds finish - initial
			}
		}
		
		return profit;
		
	}
}
