Langman.DataStructures.TreeDictionary
==============

A fast .NET Red-Black Tree implementing IDictionary&lt;TKey,TValue&gt;

The API is identical to the .NET SortedDictionary.

State: Alpha (not complete, just basic insert/remove/query operations)

Some quick performance comparisons (n = 5,000,000)

<table>
<tr><th>Implementation</th><th>Insertion</th><th>Query</th><th>In-order Enumeration</th><th>Removal</th></tr>
<tr>
<td>1. System.Collections.Generic.SortedDictionary</td>
<td>1.447&#956;s</td>
<td>0.782&#956;s</td>
<td>0.098&#956;s</td>
<td>1.227&#956;s</td>
</tr>
<tr>
<td>2. <strong>Langman.DataStructures.TreeDictionary</strong></td>
<td>0.801&#956;s (55.33%)</td>
<td>0.707&#956;s (90.44%)</td>
<td>0.088&#956;s (89.64%)</td>
<td>0.855&#956;s (69.67%)</td>
</tr>
</table>
