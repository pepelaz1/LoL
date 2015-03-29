<HTML>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
<link href="css/style.css" rel="stylesheet" type="text/css">
<title>League Scanner</title>
</head>
<body>
	<table class="summoner_body">
		<tr height="10%">
			<td>
				<table width="100%">
					<TR height="80%">
						<td valign="top">
							<TABLE cellspacing="0" cellpadding="0" width="100%">
								<TR class="header">
									<TD width="15%"><img height="70" src="Resource/Logo.png"></TD>
									<td width="85%" />
								</TR>
							</TABLE>
						</td>
					</TR>
					<tr height="20%">
						<td>
							<table width="100%" cellspacing="0" cellpadding="0">
								<tr>
									<td width="5%" align="left">Home</td>
									<td width="5%" align="left">Summoner</td>
									<td width="5%" align="left">
									<?php echo '<a href="game.php?summonerName='.htmlspecialchars($_REQUEST['summonerName']).'">Game</a>'?>
									</td>
									<td width="85%" align="right">Summoner:<?php echo htmlspecialchars($_REQUEST['summonerName'])?></td>
								</TR>
							</table>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr height="90%">
			<td>
				<table class="summoner_body_main">
					<tr height="1%">
						<td>
							<table class="sb_main_table">
								<tr>
									<td class="sb_td" align="left">Top five champions</td>
								</TR>
							</table>
						</td>
					</tr>
					<tr height="20%">
						<td>
							<table class="sb_main_table" height="100%">
								<tr>
									<td align="center" class="sb_td">Champion1</td>
									<td align="center" class="sb_td">Champion2</td>
									<td align="center" class="sb_td">Champion3</td>
									<td align="center" class="sb_td">Champion4</td>
									<td align="center" class="sb_td">Champion5</td>
								</TR>
							</table>
						</td>

					</tr>
					<tr height="1%">
						<td>
							<table class="sb_main_table">
								<tr>
									<td class="sb_td" align="left" width="50%" height="100%">Ranked
										Ratings</td>
									<td class="sb_td" align="left" width="50%" height="100%">Statistics</td>
								</TR>
							</table>
						</td>
					</tr>
					<tr height="52%">
						<td>
							<table class="sb_main_table" height="100%">
								<tr>
									<td width="50%">
										<table class="sb_main_table" height="100%">
											<tr>
												<td style="vertical-align: top; text-align: center"
													class="sb_td" height="100%">Team 3v3</td>
												<td style="vertical-align: top; text-align: center"
													class="sb_td" height="100%">Solo 5v5</td>
												<td style="vertical-align: top; text-align: center"
													class="sb_td" height="100%">Team 5v5</td>
											</tr>
										</table>
									</td>
									<TD width="50%">
										<table class="sb_main_table" height="100%">
											<tr height="70%">
												<td>
													<table class="sb_main_table" height="100%">
														<tr height="10%">
															<td align="center" class="sb_td">Ranked 5v5:</td>
															<td align="center" class="sb_td">Normal 5v5:</td>
															<td align="center" class="sb_td">Ward score:</td>
														</tr>
														<tr height="90%">
															<td width="30%" class="sb_td"></td>
															<td width="30%" class="sb_td"></td>
															<td width="30%" class="sb_td"></td>
														</tr>
													</table>
												</td>
											</tr>
											<tr height="30%">
												<td>
													<table class="sb_main_table" height="100%">
														<tr height="10%">
															<td align="left" width="100%" class="sb_td">Ranked
																Averages:</td>
														</tr>
														<tr height="90%">
															<td width="100%" class="sb_td"></td>
														</tr>
													</table>
												</TD>
											</tr>
										</table>
									</TD>
								</tr>
							</table>
						</td>
					</tr>
					<tr height="1%">
						<td align="left" width="100%" class="sb_td">Recent Champss:</td>
					</tr>
					<tr height="25%">
						<td>
							<table class="sb_main_table" height="100%">
								<tr>
									<td align="center" class="sb_td">RChampion1</td>
									<td align="center" class="sb_td">RChampion2</td>
									<td align="center" class="sb_td">RChampion3</td>
									<td align="center" class="sb_td">RChampion4</td>
									<td align="center" class="sb_td">RChampion5</td>
								</TR>
							</table>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</body>
</HTML>
